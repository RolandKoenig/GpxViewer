﻿using System;
using Mapsui.Logging;

namespace Mapsui.Samples.Wpf.Utilities
{
    class LogModel
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
