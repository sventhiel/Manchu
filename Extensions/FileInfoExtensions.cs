﻿using System.IO;
using System;

namespace Manchu.Extensions
{
    public static class FileInfoExtensions
    {
        public static string GetFileNameWithoutExtension(this FileInfo @this)
        {
            return Path.GetFileNameWithoutExtension(@this.FullName);
        }

        public static string GetExtension(this FileInfo @this)
        {
            return Path.GetExtension(@this.FullName);
        }
    }
}