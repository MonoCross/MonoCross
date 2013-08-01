using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MonoCross.Navigation
{
    public class MXNavigation
    {
        public IMXController Controller { get; private set; }
        public string Pattern { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }

        public MXNavigation(string pattern, IMXController controller, Dictionary<string, string> parameters)
        {
            Controller = controller;
            Pattern = pattern;
            Parameters = parameters;
            Parts = Segment.Split(Pattern);
        }

        public string RegexPattern()
        {
            return Pattern.Replace("{", "(?<").Replace("}", @">[-&\w\. ]+)");
        }

        /// <summary>
        /// Parses the specified URL for parameters and adds them to the specified dictionary.
        /// </summary>
        /// <param name="url">The URL to parse for parameters.</param>
        /// <param name="parameters">The <see cref="Dictionary&lt;TKey, TValue&gt;"/> to add the parsed parameters to.</param>
        /// <exception cref="ArgumentException">Thrown if the segment count of the <paramref name="url"/> is not equal to the segment count of this instance.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameters"/> is <c>null</c>.</exception>
        public void ExtractParameters(string url, Dictionary<string, string> parameters)
        {
            string[] urlParts = url.Split(new char[] { '/' });
            if (urlParts.Length != this.Parts.Length)
                throw new ArgumentException("URL is not valid for this match!", "url");
            else if (parameters == null)
                throw new ArgumentNullException("Parameters must have already been allocated!", "parameters");

            for (int partNumber = 0; partNumber < urlParts.Length; partNumber++)
            {
                if (Parts[partNumber].IsParameter)
                    parameters[Parts[partNumber].SegmentValue] = urlParts[partNumber];
            }
        }


        class Segment
        {
            /// <summary>
            /// Splits the specified URL into segments and returns the result.
            /// </summary>
            /// <param name="url">The URL to split into segments.</param>
            public static Segment[] Split(string url)
            {
                string[] parts = url.Split(new char[] { '/' });
                Segment[] segments = new Segment[parts.Length];
                for (int partNumber = 0; partNumber < parts.Length; partNumber++)
                    segments[partNumber] = new Segment(parts[partNumber]);
                return segments;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Segment"/> class.
            /// </summary>
            /// <param name="segment">The URL part to initialize with.</param>
            public Segment(string segment)
            {
                segment = segment.Trim();
                if (segment.Length > 1 && segment[0] == '{' && segment[segment.Length - 1] == '}')
                {
                    // should be a part field, extract the part name
                    SegmentValue = segment.Substring(1, segment.Length - 2);
                    IsParameter = true;
                }
                else
                {
                    SegmentValue = segment;
                    IsParameter = false;
                }
            }

            /// <summary>
            /// Gets whether this instance represents a parameter in the URL.
            /// </summary>
            public bool IsParameter { get; private set; }

            /// <summary>
            /// Gets the value of this instance.
            /// </summary>
            public string SegmentValue { get; private set; }
            //RegEx TypeValidator; // could be added in the future to allow paths such as Customer/{Number:[0-9]* or Customer/{Name:[A-Za-z ]*}
        }

        Segment[] Parts { get; set; }
	}

    public class NavigationList : List<MXNavigation>
    {
        public event NavigationAddedDelegate Added;

        public void Add(string pattern, IMXController controller)
        {
            this.Add(pattern, controller, new Dictionary<string, string>());
        }

        public IMXController GetControllerForPattern(string pattern)
        {
            return this.Contains(pattern) ? this.Where(m => m.Pattern == pattern).First().Controller : null;
        }

        public String GetPatternForModelType(Type modelType)
        {
            return this.Where(m => m.Controller.ModelType == modelType).First().Pattern;
        }

        public bool Contains(string pattern)
        {
            return this.Where(m => m.Pattern == pattern).Count() > 0;
        }

        public void Add(string pattern, IMXController controller, Dictionary<string, string> parameters)
        {
#if DROID
                Android.Util.Log.Debug("NavigationList", "Adding: '" + pattern + "'");
#endif
            // Enforce uniqueness
            MXNavigation currentMatch = this.Where(m => m.Pattern == pattern).FirstOrDefault();
            if (currentMatch != null)
            {
#if DEBUG
                string text = string.Format("MapUri \"{0}\" is already matched to Controller type {1}",
                                                                        pattern, currentMatch.Controller);
                throw new Exception(text);
#else
                    return;
#endif
            }

            var mxNavItem = new MXNavigation(pattern, controller, parameters);
            this.Add(mxNavItem);

            var e = Added;
            if (e != null) e(this, new NavAddedEventArgs(mxNavItem));

        }

        public MXNavigation MatchUrl(string url)
        {
            return this.FirstOrDefault(pattern => Regex.Match(url, pattern.RegexPattern()).Value == url);
        }
    }

    public delegate void NavigationAddedDelegate(NavigationList sender, NavAddedEventArgs e);

    public class NavAddedEventArgs : EventArgs
    {
        public NavAddedEventArgs(MXNavigation navItem)
        {
            NavigationItem = navItem;
        }
        public MXNavigation NavigationItem;
    }
}
