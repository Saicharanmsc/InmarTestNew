using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using InmarTest.DataAccess;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;
using System.ComponentModel;


namespace BusinessObjects.Util
{
    public class Utils
    {
        private static Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public static int Short2Int(short n)
        {
            return int.Parse(n.ToString());
        }

        public static bool Int2Bool(int n)
        {
            if (n == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int Bool2Int(bool b)
        {
            if (b)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static string NullToString(string objToCheck)
        {
            if (objToCheck != null && objToCheck.Length == 0)
                return " ";
            else
                return objToCheck;
        }

        public static int NVL(object obj, int defVal)
        {
            try
            {
                if (obj == null || obj == System.DBNull.Value)
                    return defVal;
                else
                    return Convert.ToInt32(obj.ToString());
            }
            catch
            {
                return -1;
            }
        }

        public static string CheckNull(string s)
        {
            if (s == null)
                return string.Empty;
            else
                return s;

        }

        public static object CheckNull(object obj, SqlDbType type)
        {
            if (type == SqlDbType.DateTime || type == SqlDbType.Date)
            {
                if (obj != null && obj != System.DBNull.Value && !string.IsNullOrWhiteSpace(obj.ToString()))
                {
                    if (DateTime.Parse(obj.ToString()) == DateTime.MinValue)
                    {
                        return new DateTime(1753, 1, 1);
                    }
                }
            }
            if (obj == null || obj == System.DBNull.Value)
            {
                switch (type)
                {
                    case SqlDbType.VarChar:
                    case SqlDbType.Char:
                    case SqlDbType.Text:
                        return string.Empty;
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                        return 0.0;
                    case SqlDbType.Int:
                    case SqlDbType.BigInt:
                    case SqlDbType.SmallInt:
                    case SqlDbType.TinyInt:
                        return 0;

                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                        return new DateTime(1753, 1, 1);
                    case SqlDbType.Bit:
                        return false;

                }
                throw new Exception("Unsupported type of '" + type.ToString() + "' specified.");
            }
            else
            {
                return obj;
            }
        }
        public static object CheckNull(object obj, SqlDbType type, object returnvalue)
        {
            if (obj == null || obj == System.DBNull.Value)
            {
                switch (type)
                {
                    case SqlDbType.VarChar:
                    case SqlDbType.Char:
                    case SqlDbType.Text:
                        return returnvalue;
                    case SqlDbType.Decimal:
                    case SqlDbType.Float:
                        return returnvalue;
                    case SqlDbType.Int:
                    case SqlDbType.BigInt:
                    case SqlDbType.SmallInt:
                    case SqlDbType.TinyInt:
                        return returnvalue;

                    case SqlDbType.Date:
                    case SqlDbType.DateTime:
                        return returnvalue;

                }
                throw new Exception("Unsupported type of '" + type.ToString() + "' specified.");
            }
            else
            {
                return obj;
            }
        }

        public static string convertToUri(String str, bool removeSpace)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            
            if(str != null)
                str = str.Trim();

            if (removeSpace)
                str = Regex.Replace(str, @"\s+", "");

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ' ')
                    count++;
            }

            for (int i = 0; ; i++)
            {
                if (str[i] != ' ')
                {
                    sb.Append(str[i]);
                    count--;
                }
                else
                    sb.Append("%2b");
                if (count == 0)
                    break;
            }
            return sb.ToString();
        }
        public static string Left(string str, int n)
        {
            try
            {
                if (str.Length <= n)
                    return str;
                else
                    return str.Substring(0, n);
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string Right(string str, int n)
        {
            try
            {
                if (str.Length <= n)
                    return str;
                else
                    return str.Substring(str.Length - n, n);
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string ProperCase(string s)
        {
            try
            {
                s = s.ToLower().Trim();

                if (s.Length == 0)
                    return s;

                string sProper = "";

                char[] seps = new char[] { '-', ' ' };
                foreach (string ss in s.Split(seps))
                {
                    sProper += char.ToUpper(ss[0]);
                    sProper += (ss.Substring(1, ss.Length - 1) + '-');
                }
                return sProper.Replace("-", " ").Trim();
            }
            catch
            {
                return s;
            }
        }

        public static string FormatPhone(string s)
        {
            string Phone = String.Empty;

            if (s == null)
                return String.Empty;

            s = Regex.Replace(s, @"[^\d]", "");
            // if a ( ) is found in the string presented, return it assuming the string is already formatted
            if (s.IndexOf("(") > -1 || s.IndexOf(")") > -1) return s;

            // if a - is found return that
            if (s.IndexOf("-") > -1) return s;

            // if a - is found return that
            if (s.IndexOf("+") > -1) return s;

            //IF LENGTH > 10 THEN FORMAT AS PHONE WITH EXTENSION				
            if (s.Length > 10)
                Phone = "(" + s.Substring(0, 3) + ") " + s.Substring(3, 3) + "-" + s.Substring(6, 4) + " x" + s.Substring(10);

            // IF return value not already set, ignore extension and return that value, 
            if (Phone.Length == 0 && s.Length == 10)
            {
                Phone = "(" + s.Substring(0, 3) + ") " + s.Substring(3, 3) + "-" + s.Substring(6, 4);
            }
            else
            {
                if (Phone.Length == 0)
                    Phone = s; // catch all because we know that we didnt set the return value and the s length was not 10 or greater so let's use what we were told
            }

            return Phone;
        }

        public static XmlDocument GetObjAsXmlDocument(object obj)
        {
            XmlDocument retValue = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            serializer.Serialize(stream, obj);
            stream.Seek(0, 0);

            retValue.Load(stream);
            stream.Close();

            return retValue;
        }

        public static void WriteTextToFile(string text, string FullFileName)
        {
            StreamWriter SW;
            if (File.Exists(FullFileName))
            {
                SW = File.AppendText(FullFileName);
                SW.Write(text);
                SW.Close();
            }
            else
            {
                SW = File.CreateText(FullFileName);
                SW.Write(text);
                SW.Close();
            }
        }

        public static string TimeFormat(DateTime date)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            if (delta < 0)
            {
                return "not yet";
            }
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE)
            {
                return "a minute ago";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90 * MINUTE)
            {
                return "an hour ago";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48 * HOUR)
            {
                return "yesterday";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " days ago";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }

        public static string GetExtensionByContentType(string contentType)
        {
            switch (contentType)
            {
                case "text/plain": return "txt"; break;
                case "application/pdf": return "pdf"; break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document": return "docx"; break;
                case "application/msword": return "doc"; break;
                case "application/rtf": return "rtf"; break;
                case "text/richtext": return "rtx"; break;
                case "application/vnd.oasis.opendocument.text ": return "odt"; break;
                case "application/wordperfect": return "wpd"; break;
                default: return "doc"; break;
            }
        }

        public static string GetContentType(string fileName)
        {
            string contentType = "";
            string extension = System.IO.Path.GetExtension(fileName);

            if (!string.IsNullOrWhiteSpace(extension))
                extension = extension.Replace(".", "").ToLower();

            #region List Of Content Types

            Dictionary<string, string> mimeTypes = new Dictionary<string, string>();
            mimeTypes.Add("doc", "application/msword");
            mimeTypes.Add("pdf", "application/pdf");
            mimeTypes.Add("rt", "text/richtext");
            mimeTypes.Add("rtf", "text/richtext");
            mimeTypes.Add("rtx", "application/rtf");
            mimeTypes.Add("txt", "text/plain");
            mimeTypes.Add("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            mimeTypes.Add("3dm", "x-world/x-3dmf");
            mimeTypes.Add("3dmf", "x-world/x-3dmf");
            mimeTypes.Add("a", "application/octet-stream");
            mimeTypes.Add("aab", "application/x-authorware-bin");
            mimeTypes.Add("aam", "application/x-authorware-map");
            mimeTypes.Add("aas", "application/x-authorware-seg");
            mimeTypes.Add("abc", "text/vnd.abc");
            mimeTypes.Add("acgi", "text/html");
            mimeTypes.Add("afl", "video/animaflex");
            mimeTypes.Add("ai", "application/postscript");
            mimeTypes.Add("aif", "audio/aiff");
            mimeTypes.Add("aifc", "audio/aiff");
            mimeTypes.Add("aiff", "audio/aiff");
            mimeTypes.Add("aim", "application/x-aim");
            mimeTypes.Add("aip", "text/x-audiosoft-intra");
            mimeTypes.Add("ani", "application/x-navi-animation");
            mimeTypes.Add("aos", "application/x-nokia-9000-communicator-add-on-software");
            mimeTypes.Add("aps", "application/mime");
            mimeTypes.Add("arc", "application/octet-stream");
            mimeTypes.Add("arj", "application/arj");
            mimeTypes.Add("art", "image/x-jg");
            mimeTypes.Add("asf", "video/x-ms-asf");
            mimeTypes.Add("asm", "text/x-asm");
            mimeTypes.Add("asp", "text/asp");
            mimeTypes.Add("asx", "application/x-mplayer2");
            mimeTypes.Add("au", "audio/basic");
            mimeTypes.Add("avi", "video/avi");
            mimeTypes.Add("avs", "video/avs-video");
            mimeTypes.Add("bcpio", "application/x-bcpio");
            mimeTypes.Add("bin", "application/octet-stream");
            mimeTypes.Add("bm", "image/bmp");
            mimeTypes.Add("bmp", "image/bmp");
            mimeTypes.Add("boo", "application/book");
            mimeTypes.Add("book", "application/book");
            mimeTypes.Add("boz", "application/x-bzip2");
            mimeTypes.Add("bsh", "application/x-bsh");
            mimeTypes.Add("bz", "application/x-bzip");
            mimeTypes.Add("bz2", "application/x-bzip2");
            mimeTypes.Add("c", "text/plain");
            mimeTypes.Add("c++", "text/plain");
            mimeTypes.Add("cat", "application/vnd.ms-pki.seccat");
            mimeTypes.Add("cc", "text/plain");
            mimeTypes.Add("ccad", "application/clariscad");
            mimeTypes.Add("cco", "application/x-cocoa");
            mimeTypes.Add("cdf", "application/cdf");
            mimeTypes.Add("cer", "application/pkix-cert");
            mimeTypes.Add("cha", "application/x-chat");
            mimeTypes.Add("chat", "application/x-chat");
            mimeTypes.Add("class", "application/java");
            mimeTypes.Add("com", "application/octet-stream");
            mimeTypes.Add("conf", "text/plain");
            mimeTypes.Add("cpio", "application/x-cpio");
            mimeTypes.Add("cpp", "text/x-c");
            mimeTypes.Add("cpt", "application/x-cpt");
            mimeTypes.Add("crl", "application/pkcs-crl");
            mimeTypes.Add("css", "text/css");
            mimeTypes.Add("def", "text/plain");
            mimeTypes.Add("der", "application/x-x509-ca-cert");
            mimeTypes.Add("dif", "video/x-dv");
            mimeTypes.Add("dir", "application/x-director");
            mimeTypes.Add("dl", "video/dl");
            mimeTypes.Add("dot", "application/msword");
            mimeTypes.Add("dp", "application/commonground");
            mimeTypes.Add("drw", "application/drafting");
            mimeTypes.Add("dump", "application/octet-stream");
            mimeTypes.Add("dv", "video/x-dv");
            mimeTypes.Add("dvi", "application/x-dvi");
            mimeTypes.Add("dwf", "drawing/x-dwf (old)");
            mimeTypes.Add("dwg", "application/acad");
            mimeTypes.Add("dxf", "application/dxf");
            mimeTypes.Add("eps", "application/postscript");
            mimeTypes.Add("es", "application/x-esrehber");
            mimeTypes.Add("etx", "text/x-setext");
            mimeTypes.Add("evy", "application/envoy");
            mimeTypes.Add("exe", "application/octet-stream");
            mimeTypes.Add("f", "text/plain");
            mimeTypes.Add("f90", "text/x-fortran");
            mimeTypes.Add("fdf", "application/vnd.fdf");
            mimeTypes.Add("fif", "image/fif");
            mimeTypes.Add("fli", "video/fli");
            mimeTypes.Add("flv", "video/x-flv");
            mimeTypes.Add("for", "text/x-fortran");
            mimeTypes.Add("fpx", "image/vnd.fpx");
            mimeTypes.Add("g", "text/plain");
            mimeTypes.Add("g3", "image/g3fax");
            mimeTypes.Add("gif", "image/gif");
            mimeTypes.Add("gl", "video/gl");
            mimeTypes.Add("gsd", "audio/x-gsm");
            mimeTypes.Add("gtar", "application/x-gtar");
            mimeTypes.Add("gz", "application/x-compressed");
            mimeTypes.Add("h", "text/plain");
            mimeTypes.Add("help", "application/x-helpfile");
            mimeTypes.Add("hgl", "application/vnd.hp-hpgl");
            mimeTypes.Add("hh", "text/plain");
            mimeTypes.Add("hlp", "application/x-winhelp");
            mimeTypes.Add("htc", "text/x-component");
            mimeTypes.Add("htm", "text/html");
            mimeTypes.Add("html", "text/html");
            mimeTypes.Add("htmls", "text/html");
            mimeTypes.Add("htt", "text/webviewhtml");
            mimeTypes.Add("htx", "text/html");
            mimeTypes.Add("ice", "x-conference/x-cooltalk");
            mimeTypes.Add("ico", "image/x-icon");
            mimeTypes.Add("idc", "text/plain");
            mimeTypes.Add("ief", "image/ief");
            mimeTypes.Add("iefs", "image/ief");
            mimeTypes.Add("iges", "application/iges");
            mimeTypes.Add("igs", "application/iges");
            mimeTypes.Add("ima", "application/x-ima");
            mimeTypes.Add("imap", "application/x-httpd-imap");
            mimeTypes.Add("inf", "application/inf");
            mimeTypes.Add("ins", "application/x-internett-signup");
            mimeTypes.Add("ip", "application/x-ip2");
            mimeTypes.Add("isu", "video/x-isvideo");
            mimeTypes.Add("it", "audio/it");
            mimeTypes.Add("iv", "application/x-inventor");
            mimeTypes.Add("ivr", "i-world/i-vrml");
            mimeTypes.Add("ivy", "application/x-livescreen");
            mimeTypes.Add("jam", "audio/x-jam");
            mimeTypes.Add("jav", "text/plain");
            mimeTypes.Add("java", "text/plain");
            mimeTypes.Add("jcm", "application/x-java-commerce");
            mimeTypes.Add("jfif", "image/jpeg");
            mimeTypes.Add("jfif-tbnl", "image/jpeg");
            mimeTypes.Add("jpe", "image/jpeg");
            mimeTypes.Add("jpeg", "image/jpeg");
            mimeTypes.Add("jpg", "image/jpeg");
            mimeTypes.Add("jps", "image/x-jps");
            mimeTypes.Add("js", "application/x-javascript");
            mimeTypes.Add("jut", "image/jutvision");
            mimeTypes.Add("kar", "audio/midi");
            mimeTypes.Add("ksh", "application/x-ksh");
            mimeTypes.Add("la", "audio/nspaudio");
            mimeTypes.Add("lam", "audio/x-liveaudio");
            mimeTypes.Add("latex", "application/x-latex");
            mimeTypes.Add("lha", "application/lha");
            mimeTypes.Add("lhx", "application/octet-stream");
            mimeTypes.Add("list", "text/plain");
            mimeTypes.Add("lma", "audio/nspaudio");
            mimeTypes.Add("log", "text/plain");
            mimeTypes.Add("lsp", "application/x-lisp");
            mimeTypes.Add("lst", "text/plain");
            mimeTypes.Add("lsx", "text/x-la-asf");
            mimeTypes.Add("ltx", "application/x-latex");
            mimeTypes.Add("lzh", "application/octet-stream");
            mimeTypes.Add("lzx", "application/lzx");
            mimeTypes.Add("m", "text/plain");
            mimeTypes.Add("m1v", "video/mpeg");
            mimeTypes.Add("m2a", "audio/mpeg");
            mimeTypes.Add("m2v", "video/mpeg");
            mimeTypes.Add("m3u", "audio/x-mpequrl");
            mimeTypes.Add("man", "application/x-troff-man");
            mimeTypes.Add("map", "application/x-navimap");
            mimeTypes.Add("mar", "text/plain");
            mimeTypes.Add("mbd", "application/mbedlet");
            mimeTypes.Add("mc$", "application/x-magic-cap-package-1.0");
            mimeTypes.Add("mcd", "application/mcad");
            mimeTypes.Add("mcf", "image/vasa");
            mimeTypes.Add("mcp", "application/netmc");
            mimeTypes.Add("me", "application/x-troff-me");
            mimeTypes.Add("mht", "message/rfc822");
            mimeTypes.Add("mhtml", "message/rfc822");
            mimeTypes.Add("mid", "audio/midi");
            mimeTypes.Add("midi", "audio/midi");
            mimeTypes.Add("mif", "application/x-frame");
            mimeTypes.Add("mime", "message/rfc822");
            mimeTypes.Add("mjf", "audio/x-vnd.audioexplosion.mjuicemediafile");
            mimeTypes.Add("mjpg", "video/x-motion-jpeg");
            mimeTypes.Add("mm", "application/base64");
            mimeTypes.Add("mme", "application/base64");
            mimeTypes.Add("mod", "audio/mod");
            mimeTypes.Add("moov", "video/quicktime");
            mimeTypes.Add("mov", "video/quicktime");
            mimeTypes.Add("movie", "video/x-sgi-movie");
            mimeTypes.Add("mp2", "audio/mpeg");
            mimeTypes.Add("mp3", "audio/mpeg3");
            mimeTypes.Add("mpa", "audio/mpeg");
            mimeTypes.Add("mpc", "application/x-project");
            mimeTypes.Add("mpe", "video/mpeg");
            mimeTypes.Add("mpeg", "video/mpeg");
            mimeTypes.Add("mpg", "video/mpeg");
            mimeTypes.Add("mpga", "audio/mpeg");
            mimeTypes.Add("mpp", "application/vnd.ms-project");
            mimeTypes.Add("mpt", "application/x-project");
            mimeTypes.Add("mpv", "application/x-project");
            mimeTypes.Add("mpx", "application/x-project");
            mimeTypes.Add("mrc", "application/marc");
            mimeTypes.Add("ms", "application/x-troff-ms");
            mimeTypes.Add("mv", "video/x-sgi-movie");
            mimeTypes.Add("my", "audio/make");
            mimeTypes.Add("mzz", "application/x-vnd.audioexplosion.mzz");
            mimeTypes.Add("nap", "image/naplps");
            mimeTypes.Add("naplps", "image/naplps");
            mimeTypes.Add("nc", "application/x-netcdf");
            mimeTypes.Add("ncm", "application/vnd.nokia.configuration-message");
            mimeTypes.Add("nif", "image/x-niff");
            mimeTypes.Add("niff", "image/x-niff");
            mimeTypes.Add("nix", "application/x-mix-transfer");
            mimeTypes.Add("nsc", "application/x-conference");
            mimeTypes.Add("nvd", "application/x-navidoc");
            mimeTypes.Add("o", "application/octet-stream");
            mimeTypes.Add("oda", "application/oda");
            mimeTypes.Add("omc", "application/x-omc");
            mimeTypes.Add("omcd", "application/x-omcdatamaker");
            mimeTypes.Add("omcr", "application/x-omcregerator");
            mimeTypes.Add("p", "text/x-pascal");
            mimeTypes.Add("p10", "application/pkcs10");
            mimeTypes.Add("p12", "application/pkcs-12");
            mimeTypes.Add("p7a", "application/x-pkcs7-signature");
            mimeTypes.Add("p7c", "application/pkcs7-mime");
            mimeTypes.Add("pas", "text/pascal");
            mimeTypes.Add("pbm", "image/x-portable-bitmap");
            mimeTypes.Add("pcl", "application/vnd.hp-pcl");
            mimeTypes.Add("pct", "image/x-pict");
            mimeTypes.Add("pcx", "image/x-pcx");
            mimeTypes.Add("pfunk", "audio/make");
            mimeTypes.Add("pgm", "image/x-portable-graymap");
            mimeTypes.Add("pic", "image/pict");
            mimeTypes.Add("pict", "image/pict");
            mimeTypes.Add("pkg", "application/x-newton-compatible-pkg");
            mimeTypes.Add("pko", "application/vnd.ms-pki.pko");
            mimeTypes.Add("pl", "text/plain");
            mimeTypes.Add("plx", "application/x-pixclscript");
            mimeTypes.Add("pm", "image/x-xpixmap");
            mimeTypes.Add("png", "image/png");
            mimeTypes.Add("pnm", "application/x-portable-anymap");
            mimeTypes.Add("pot", "application/mspowerpoint");
            mimeTypes.Add("pov", "model/x-pov");
            mimeTypes.Add("ppa", "application/vnd.ms-powerpoint");
            mimeTypes.Add("ppm", "image/x-portable-pixmap");
            mimeTypes.Add("pps", "application/mspowerpoint");
            mimeTypes.Add("ppt", "application/mspowerpoint");
            mimeTypes.Add("ppz", "application/mspowerpoint");
            mimeTypes.Add("pre", "application/x-freelance");
            mimeTypes.Add("prt", "application/pro_eng");
            mimeTypes.Add("ps", "application/postscript");
            mimeTypes.Add("psd", "application/octet-stream");
            mimeTypes.Add("pvu", "paleovu/x-pv");
            mimeTypes.Add("pwz", "application/vnd.ms-powerpoint");
            mimeTypes.Add("py", "text/x-script.phyton");
            mimeTypes.Add("pyc", "applicaiton/x-bytecode.python");
            mimeTypes.Add("qcp", "audio/vnd.qcelp");
            mimeTypes.Add("qd3", "x-world/x-3dmf");
            mimeTypes.Add("qd3d", "x-world/x-3dmf");
            mimeTypes.Add("qif", "image/x-quicktime");
            mimeTypes.Add("qt", "video/quicktime");
            mimeTypes.Add("qtc", "video/x-qtc");
            mimeTypes.Add("qti", "image/x-quicktime");
            mimeTypes.Add("qtif", "image/x-quicktime");
            mimeTypes.Add("ra", "audio/x-pn-realaudio");
            mimeTypes.Add("ram", "audio/x-pn-realaudio");
            mimeTypes.Add("ras", "application/x-cmu-raster");
            mimeTypes.Add("rast", "image/cmu-raster");
            mimeTypes.Add("rexx", "text/x-script.rexx");
            mimeTypes.Add("rf", "image/vnd.rn-realflash");
            mimeTypes.Add("rgb", "image/x-rgb");
            mimeTypes.Add("rm", "application/vnd.rn-realmedia");
            mimeTypes.Add("rmi", "audio/mid");
            mimeTypes.Add("rmm", "audio/x-pn-realaudio");
            mimeTypes.Add("rmp", "audio/x-pn-realaudio");
            mimeTypes.Add("rng", "application/ringing-tones");
            mimeTypes.Add("rnx", "application/vnd.rn-realplayer");
            mimeTypes.Add("roff", "application/x-troff");
            mimeTypes.Add("rp", "image/vnd.rn-realpix");
            mimeTypes.Add("rpm", "audio/x-pn-realaudio-plugin");

            mimeTypes.Add("rv", "video/vnd.rn-realvideo");
            mimeTypes.Add("s", "text/x-asm");
            mimeTypes.Add("s3m", "audio/s3m");
            mimeTypes.Add("saveme", "application/octet-stream");
            mimeTypes.Add("sbk", "application/x-tbook");
            mimeTypes.Add("scm", "application/x-lotusscreencam");
            mimeTypes.Add("sdml", "text/plain");
            mimeTypes.Add("sdp", "application/sdp");
            mimeTypes.Add("sdr", "application/sounder");
            mimeTypes.Add("sea", "application/sea");
            mimeTypes.Add("set", "application/set");
            mimeTypes.Add("sgm", "text/sgml");
            mimeTypes.Add("sgml", "text/sgml");
            mimeTypes.Add("sh", "application/x-bsh");
            mimeTypes.Add("shtml", "text/html");
            mimeTypes.Add("sid", "audio/x-psid");
            mimeTypes.Add("sit", "application/x-sit");
            mimeTypes.Add("skd", "application/x-koan");
            mimeTypes.Add("skm", "application/x-koan");
            mimeTypes.Add("skp", "application/x-koan");
            mimeTypes.Add("skt", "application/x-koan");
            mimeTypes.Add("sl", "application/x-seelogo");
            mimeTypes.Add("smi", "application/smil");
            mimeTypes.Add("smil", "application/smil");
            mimeTypes.Add("snd", "audio/basic");
            mimeTypes.Add("sol", "application/solids");
            mimeTypes.Add("spc", "application/x-pkcs7-certificates");
            mimeTypes.Add("spl", "application/futuresplash");
            mimeTypes.Add("spr", "application/x-sprite");
            mimeTypes.Add("sprite", "application/x-sprite");
            mimeTypes.Add("src", "application/x-wais-source");
            mimeTypes.Add("ssi", "text/x-server-parsed-html");
            mimeTypes.Add("ssm", "application/streamingmedia");
            mimeTypes.Add("sst", "application/vnd.ms-pki.certstore");
            mimeTypes.Add("step", "application/step");
            mimeTypes.Add("stl", "application/sla");
            mimeTypes.Add("stp", "application/step");
            mimeTypes.Add("sv4cpio", "application/x-sv4cpio");
            mimeTypes.Add("sv4crc", "application/x-sv4crc");
            mimeTypes.Add("svf", "image/vnd.dwg");
            mimeTypes.Add("svr", "application/x-world");
            mimeTypes.Add("swf", "application/x-shockwave-flash");
            mimeTypes.Add("t", "application/x-troff");
            mimeTypes.Add("talk", "text/x-speech");
            mimeTypes.Add("tar", "application/x-tar");
            mimeTypes.Add("tbk", "application/toolbook");
            mimeTypes.Add("tcl", "application/x-tcl");
            mimeTypes.Add("tcsh", "text/x-script.tcsh");
            mimeTypes.Add("tex", "application/x-tex");
            mimeTypes.Add("texi", "application/x-texinfo");
            mimeTypes.Add("texinfo", "application/x-texinfo");
            mimeTypes.Add("text", "text/plain");
            mimeTypes.Add("tgz", "application/x-compressed");
            mimeTypes.Add("tif", "image/tiff");
            mimeTypes.Add("tr", "application/x-troff");
            mimeTypes.Add("tsi", "audio/tsp-audio");
            mimeTypes.Add("tsp", "audio/tsplayer");
            mimeTypes.Add("tsv", "text/tab-separated-values");
            mimeTypes.Add("turbot", "image/florian");

            mimeTypes.Add("uil", "text/x-uil");
            mimeTypes.Add("uni", "text/uri-list");
            mimeTypes.Add("unis", "text/uri-list");
            mimeTypes.Add("unv", "application/i-deas");
            mimeTypes.Add("uri", "text/uri-list");
            mimeTypes.Add("uris", "text/uri-list");
            mimeTypes.Add("ustar", "application/x-ustar");
            mimeTypes.Add("uu", "application/octet-stream");
            mimeTypes.Add("vcd", "application/x-cdlink");
            mimeTypes.Add("vcs", "text/x-vcalendar");
            mimeTypes.Add("vda", "application/vda");
            mimeTypes.Add("vdo", "video/vdo");
            mimeTypes.Add("vew", "application/groupwise");
            mimeTypes.Add("viv", "video/vivo");
            mimeTypes.Add("vivo", "video/vivo");
            mimeTypes.Add("vmd", "application/vocaltec-media-desc");
            mimeTypes.Add("vmf", "application/vocaltec-media-file");
            mimeTypes.Add("voc", "audio/voc");
            mimeTypes.Add("vos", "video/vosaic");
            mimeTypes.Add("vox", "audio/voxware");
            mimeTypes.Add("vqe", "audio/x-twinvq-plugin");
            mimeTypes.Add("vqf", "audio/x-twinvq");
            mimeTypes.Add("vql", "audio/x-twinvq-plugin");
            mimeTypes.Add("vrml", "application/x-vrml");
            mimeTypes.Add("vrt", "x-world/x-vrt");
            mimeTypes.Add("vsd", "application/x-visio");
            mimeTypes.Add("vst", "application/x-visio");
            mimeTypes.Add("vsw", "application/x-visio");
            mimeTypes.Add("w60", "application/wordperfect6.0");
            mimeTypes.Add("w61", "application/wordperfect6.1");
            mimeTypes.Add("w6w", "application/msword");
            mimeTypes.Add("wav", "audio/wav");
            mimeTypes.Add("wb1", "application/x-qpro");
            mimeTypes.Add("wbmp", "image/vnd.wap.wbmp");
            mimeTypes.Add("web", "application/vnd.xara");
            mimeTypes.Add("wiz", "application/msword");
            mimeTypes.Add("wk1", "application/x-123");
            mimeTypes.Add("wmf", "windows/metafile");
            mimeTypes.Add("wml", "text/vnd.wap.wml");
            mimeTypes.Add("wmlc", "application/vnd.wap.wmlc");
            mimeTypes.Add("wmls", "text/vnd.wap.wmlscript");
            mimeTypes.Add("wmlsc", "application/vnd.wap.wmlscriptc");
            mimeTypes.Add("word", "application/msword");
            mimeTypes.Add("wp", "application/wordperfect");
            mimeTypes.Add("wp5", "application/wordperfect");
            mimeTypes.Add("wp6", "application/wordperfect");
            mimeTypes.Add("wpd", "application/wordperfect");
            mimeTypes.Add("wq1", "application/x-lotus");
            mimeTypes.Add("wri", "application/mswrite");
            mimeTypes.Add("wrl", "application/x-world");
            mimeTypes.Add("wrz", "model/vrml");
            mimeTypes.Add("wsc", "text/scriplet");
            mimeTypes.Add("wsrc", "application/x-wais-source");
            mimeTypes.Add("wtk", "application/x-wintalk");
            mimeTypes.Add("xbm", "image/x-xbitmap");
            mimeTypes.Add("xdr", "video/x-amt-demorun");
            mimeTypes.Add("xgz", "xgl/drawing");
            mimeTypes.Add("xif", "image/vnd.xiff");
            mimeTypes.Add("xl", "application/excel");
            mimeTypes.Add("xla", "application/excel");
            mimeTypes.Add("xlb", "application/excel");
            mimeTypes.Add("xlc", "application/excel");
            mimeTypes.Add("xld", "application/excel");
            mimeTypes.Add("xlk", "application/excel");
            mimeTypes.Add("xll", "application/excel");
            mimeTypes.Add("xlm", "application/excel");
            mimeTypes.Add("xls", "application/excel");
            mimeTypes.Add("xlsx", "application/excel");
            mimeTypes.Add("xlt", "application/excel");
            mimeTypes.Add("xlv", "application/excel");
            mimeTypes.Add("xlw", "application/excel");
            mimeTypes.Add("xm", "audio/xm");
            mimeTypes.Add("xml", "text/xml");
            mimeTypes.Add("xmz", "xgl/movie");
            mimeTypes.Add("xpix", "application/x-vnd.ls-xpix");
            mimeTypes.Add("xpm", "image/x-xpixmap");
            mimeTypes.Add("x-png", "image/png");
            mimeTypes.Add("xsr", "video/x-amt-showrun");
            mimeTypes.Add("xwd", "image/x-xwd");
            mimeTypes.Add("xyz", "chemical/x-pdb");
            mimeTypes.Add("z", "application/x-compress");
            mimeTypes.Add("zip", "application/x-compressed");
            mimeTypes.Add("zoo", "application/octet-stream");
            mimeTypes.Add("zsh", "text/x-script.zsh");

            #endregion

            mimeTypes.TryGetValue(extension, out contentType);

            return contentType;
        }

        public static object ReflectPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }

        public static string RemoveTroublesomeCharacters(string inString)
        {
            if (inString != null)
            {
                inString = inString.Replace("&#xa0;", "");
                inString = inString.Replace("&#xad;", "");
            }

            return inString;
        }

        public static string GetYearsOfExperienceString(string MinYearsOfExperiance, string MaxYearsOfExperiance)
        {
            //get years of experiance string
            string YearsOfExperiance = string.Empty;
            if (!string.IsNullOrWhiteSpace(MinYearsOfExperiance) && string.IsNullOrWhiteSpace(MaxYearsOfExperiance))
                YearsOfExperiance += ">" + MinYearsOfExperiance;

            if (string.IsNullOrWhiteSpace(MinYearsOfExperiance) && !string.IsNullOrWhiteSpace(MaxYearsOfExperiance))
                YearsOfExperiance += "<" + MaxYearsOfExperiance;

            if (!string.IsNullOrWhiteSpace(MinYearsOfExperiance) && !string.IsNullOrWhiteSpace(MaxYearsOfExperiance))
                YearsOfExperiance += MinYearsOfExperiance + "-" + MaxYearsOfExperiance;

            if (!string.IsNullOrWhiteSpace(MinYearsOfExperiance) && !string.IsNullOrWhiteSpace(MaxYearsOfExperiance) && MinYearsOfExperiance == MaxYearsOfExperiance)
                YearsOfExperiance = MaxYearsOfExperiance;

            return YearsOfExperiance;
        }

        public static string FormatNotes(string input)
        {
            //if (input != null)
            //{
            //    input = input.Replace("\n", "<br />");
            //}

            return input;

        }

        public static string GetResumeFileName(string strFileName, DateTime? strSubmitDate, string strCandidateId)
        {
            StringBuilder strbFileName = new StringBuilder();
            try
            {
                if (strSubmitDate.HasValue)
                    strbFileName.Append(strSubmitDate.Value.Month + "" + strSubmitDate.Value.Day + "" + strSubmitDate.Value.Year + "_");
                if (!String.IsNullOrEmpty(strFileName))
                {
                    if (strFileName.Contains("'\'"))
                    {
                        strFileName = strFileName.Substring(strFileName.LastIndexOf("'\'") + 1);
                    }

                    strbFileName.Append(RemoveCharactersFromString(strFileName));
                }
                else
                {
                    strbFileName.Append(strCandidateId);
                }

                return strbFileName.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public static string RemoveCharactersFromString(string fName)
        {
            string retVal = fName;
            retVal = retVal.Replace("<", "");
            retVal = retVal.Replace(">", "");
            retVal = retVal.Replace("&", "");
            return retVal;
        }

        public static List<T> DataTableToList<T>(  DataTable table ) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

       
    }
}
