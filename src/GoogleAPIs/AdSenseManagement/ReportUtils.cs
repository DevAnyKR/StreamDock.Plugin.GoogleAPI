﻿/*
Copyright 2021 Google Inc

Licensed under the Apache License, Version 2.0(the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;

using Google.Apis.Adsense.v2.Data;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// Collection of utilities to display and modify reports
    /// </summary>
    public static class ReportUtils
    {
        public const string DATEPATTERN = "yyyy-MM-dd";
        public const string MONTHPATTERN = "yyyy-MM";

        /// <summary>
        /// Displays the headers for the report.
        /// </summary>
        /// <param name="headers">The list of headers to be displayed</param>
        public static void DisplayHeaders(IList<Header> headers)
        {
            foreach (var header in headers)
            {
                Console.Write("{0, -25}", header.Name);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Displays a list of rows for the report.
        /// </summary>
        /// <param name="rows">The list of rows to display.</param>
        public static void DisplayRows(IList<Row> rows)
        {
            foreach (var row in rows)
            {
                foreach (var cell in row.Cells)
                {
                    Console.Write("{0, -25}", cell.Value);
                }

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Escape special characters for a parameter being used in a filter.
        /// </summary>
        /// <param name="parameter">The parameter to be escaped.</param>
        /// <returns>The escaped parameter.</returns>
        public static string EscapeFilterParameter(string parameter)
        {
            return parameter.Split('/').Last().Replace("\\", "\\\\").Replace(",", "\\,");
        }
    }
}