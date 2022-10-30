using System;
using System.Buffers;
using System.Globalization;
using System.Text.Json;

namespace Tiny.RestClient
{
    /// <summary>
    /// This class was took from this repository below:
    /// https://github.com/YohDeadfall/Yoh.Text.Json.NamingPolicies/blob/main/src/JsonNamingPolicies.cs
    /// </summary>
    internal abstract class JsonSeparatorNamingPolicy : JsonNamingPolicy
    {
        private readonly bool _lowercase;
        private readonly char _separator;

        internal JsonSeparatorNamingPolicy(bool lowercase, char separator)
        {
            _lowercase = lowercase;
            _separator = separator;
        }

        public override string ConvertName(string name)
        {
            // Rented buffer 20% longer that the input.
            var rentedBufferLength = (12 * name.Length) / 10;
            var rentedBuffer = rentedBufferLength > JsonConstants.StackallocCharThreshold
                ? ArrayPool<char>.Shared.Rent(rentedBufferLength)
                : null;

            var resultUsedLength = 0;
            Span<char> result = rentedBuffer is null
                ? stackalloc char[JsonConstants.StackallocCharThreshold]
                : rentedBuffer;

            void ExpandBuffer(ref Span<char> result)
            {
                var newBuffer = ArrayPool<char>.Shared.Rent(result.Length * 2);

                result.CopyTo(newBuffer);

                if (rentedBuffer is not null)
                {
                    result.Slice(0, resultUsedLength).Clear();
                    ArrayPool<char>.Shared.Return(rentedBuffer);
                }

                rentedBuffer = newBuffer;
                result = rentedBuffer;
            }

            void WriteWord(ReadOnlySpan<char> word, ref Span<char> result)
            {
                if (word.IsEmpty)
                {
                    return;
                }

                int written;
                while (true)
                {
                    var destinationOffset = resultUsedLength != 0
                        ? resultUsedLength + 1
                        : resultUsedLength;

                    if (destinationOffset < result.Length)
                    {
                        var destination = result.Slice(destinationOffset);

                        written = _lowercase
                            ? word.ToLowerInvariant(destination)
                            : word.ToUpperInvariant(destination);

                        if (written > 0)
                        {
                            break;
                        }
                    }

                    ExpandBuffer(ref result);
                }

                if (resultUsedLength != 0)
                {
                    result[resultUsedLength] = _separator;
                    resultUsedLength += 1;
                }

                resultUsedLength += written;
            }

            int first = 0;
            var chars = name.AsSpan();
            var previousCategory = CharCategory.Boundary;
            for (int index = 0; index < chars.Length; index++)
            {
                var current = chars[index];
                var currentCategoryUnicode = char.GetUnicodeCategory(current);
                if (currentCategoryUnicode == UnicodeCategory.SpaceSeparator ||
                    (currentCategoryUnicode >= UnicodeCategory.ConnectorPunctuation &&
                    currentCategoryUnicode <= UnicodeCategory.OtherPunctuation))
                {
                    WriteWord(chars.Slice(first, index - first), ref result);

                    previousCategory = CharCategory.Boundary;
                    first = index + 1;

                    continue;
                }

                if (index + 1 < chars.Length)
                {
                    var next = chars[index + 1];
                    var currentCategory = currentCategoryUnicode switch
                    {
                        UnicodeCategory.LowercaseLetter => CharCategory.Lowercase,
                        UnicodeCategory.UppercaseLetter => CharCategory.Uppercase,
                        _ => previousCategory
                    };

                    if ((currentCategory == CharCategory.Lowercase && char.IsUpper(next)) ||
                        next == '_')
                    {
                        WriteWord(chars.Slice(first, index - first + 1), ref result);

                        previousCategory = CharCategory.Boundary;
                        first = index + 1;

                        continue;
                    }

                    if (previousCategory == CharCategory.Uppercase &&
                        currentCategoryUnicode == UnicodeCategory.UppercaseLetter &&
                        char.IsLower(next))
                    {
                        WriteWord(chars.Slice(first, index - first), ref result);

                        previousCategory = CharCategory.Boundary;
                        first = index;

                        continue;
                    }

                    previousCategory = currentCategory;
                }
            }

            WriteWord(chars.Slice(first), ref result);

            name = result.Slice(0, resultUsedLength).ToString();

            if (rentedBuffer is not null)
            {
                result.Slice(0, resultUsedLength).Clear();
                ArrayPool<char>.Shared.Return(rentedBuffer);
            }

            return name;
        }

        private enum CharCategory
        {
            Boundary,
            Lowercase,
            Uppercase,
        }
    }

    internal static class JsonConstants
    {
        public const int StackallocByteThreshold = 256;
        public const int StackallocCharThreshold = StackallocByteThreshold / 2;
    }
}
