﻿using System.IO;

namespace Tiny.RestClient
{
    /// <summary>
    /// Interface IMultiPartFromDataRequest.
    /// </summary>
    public interface IMultipartFromDataRequest
    {
        /// <summary>
        /// Adds a byte array as content.
        /// </summary>
        /// <param name="data">The content.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>The current request.</returns>
        /// <exception cref="System.ArgumentNullException">thrown when data is null.</exception>
        IMultiPartFromDataExecutableRequest AddByteArray(byte[] data, string name = null, string fileName = null, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="data">The content.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>The current request.</returns>
        /// <exception cref="System.ArgumentNullException">thrown when data is null.</exception>
        IMultiPartFromDataExecutableRequest AddString(string data, string name = null, string fileName = null, string contentType = "text/plain");

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="data">The content.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>The current request.</returns>
        /// <exception cref="System.ArgumentNullException">thrown when data is null.</exception>
        IMultiPartFromDataExecutableRequest AddStream(Stream data, string name = null, string fileName = null, string contentType = "application/octet-stream");

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <typeparam name="TContent">The type of the t content.</typeparam>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="serializer">Override the default serializer setted on the client. If null use default serializer.</param>
        /// <param name="compression">Add compresion system use ton compress content.</param>
        /// <returns>The current request.</returns>
        /// <exception cref="System.ArgumentNullException">thrown when content is null.</exception>
        IMultiPartFromDataExecutableRequest AddContent<TContent>(TContent content, string name = null, string fileName = null, IFormatter serializer = null, ICompression compression = null)
            where TContent : class;

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>The current request.</returns>
        /// <exception cref="System.ArgumentNullException">thrown when content is null.</exception>
        IMultiPartFromDataExecutableRequest AddFileContent(FileInfo content, string name, string fileName, string contentType);

        /// <summary>
        /// Adds the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="contentType">The content type of the file.</param>
        /// <returns>The current request.</returns>
        /// <exception cref="System.ArgumentNullException">thrown when content is null.</exception>
        IMultiPartFromDataExecutableRequest AddFileContent(FileInfo content, string contentType);
    }
}
