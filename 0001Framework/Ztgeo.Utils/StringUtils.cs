using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ztgeo.Utils
{
	public static class StringUtils
	{ 
		public static string WithoutDiacritics(this string value)
		{
			value = value.Normalize(NormalizationForm.FormD);
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			foreach (char c in value)
			{
				if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
		 
		public static string WithoutInvalidChars(this string s)
		{
			return s.WithoutInvalidChars(new char?('_'));
		}
		 
		

		public static string WithoutInvalidChars(this string s, char? replaceChar)
		{
			return s.WithoutInvalidChars(replaceChar, new Func<char, bool>(StringUtils.IsValidChar));
		}
		 
		public static string WithoutInvalidChars(this string s, char? replaceChar, Func<char, bool> isValidChar)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in s.WithoutDiacritics())
			{
				if (isValidChar(c))
				{
					stringBuilder.Append(c);
				}
				else if (replaceChar != null && !StringUtils.CharsToRemoveInsteadOfReplacing.Contains(c))
				{
					stringBuilder.Append(replaceChar.Value);
				}
			}
			return stringBuilder.ToString();
		}
		 
		public static bool IsValidChar(char c)
		{
			return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '_';
		}
		 
		public static bool ContainsIgnoreCase(this string str, string subStr)
		{
			return str.IndexOf(subStr, StringComparison.OrdinalIgnoreCase) != -1;
		}
		 
		public static string SuffixIfNotEmpty(this string s, string suffix)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s + suffix;
			}
			return s;
		}
		 
		private static Regex CommentsRegex
		{
			get
			{
				if (StringUtils.blockCommentsRegex == null)
				{
					StringUtils.blockCommentsRegex = new Regex("/\\*(?>(?:(?>[^*]+)|\\*(?!/))*)\\*/", RegexOptions.Compiled);
				}
				return StringUtils.blockCommentsRegex;
			}
		}
		 
		private static Regex LineCommentsRegex
		{
			get
			{
				if (StringUtils.lineCommentsRegex == null)
				{
					StringUtils.lineCommentsRegex = new Regex("(\\/\\/.*$)", RegexOptions.Multiline | RegexOptions.Compiled);
				}
				return StringUtils.lineCommentsRegex;
			}
		}
		 
		private static Regex StringsRegex
		{
			get
			{
				if (StringUtils.stringsRegex == null)
				{
					StringUtils.stringsRegex = new Regex("\"([^\"\\\\]|\\\\.)*\"|'([^'\\\\]|\\\\.)*'", RegexOptions.Compiled);
				}
				return StringUtils.stringsRegex;
			}
		}

		public static Regex ValidQueryForCountSingleLineRegex
		{
			get
			{
				if (StringUtils.validQueryForCountSingleLineRegex == null)
				{
					return StringUtils.validQueryForCountSingleLineRegex = new Regex(StringUtils.validQueryForCountPattern, StringUtils.commonOptions | RegexOptions.Singleline);
				}
				return StringUtils.validQueryForCountSingleLineRegex;
			}
		}
		 
		public static Regex ValidQueryForCountMultiLineRegex
		{
			get
			{
				if (StringUtils.validQueryForCountMultiLineRegex == null)
				{
					return StringUtils.validQueryForCountMultiLineRegex = new Regex(StringUtils.validQueryForCountPattern, StringUtils.commonOptions | RegexOptions.Multiline);
				}
				return StringUtils.validQueryForCountMultiLineRegex;
			}
		}
		 
		public static string WithoutBlockComments(string text)
		{
			return StringUtils.WithoutBlockComments(text, (Match match) => string.Empty);
		}
		 
		public static string WithoutBlockComments(string text, MatchEvaluator eval)
		{
			return StringUtils.CommentsRegex.Replace(text, eval);
		}
		 
		public static void ForEachBlockComment(string text, Action<Match> eval)
		{
			foreach (object obj in StringUtils.CommentsRegex.Matches(text))
			{
				Match obj2 = (Match)obj;
				eval(obj2);
			}
		}
		 
		public static string WithoutLineComments(string text)
		{
			return StringUtils.LineCommentsRegex.Replace(text, (Match match) => string.Empty);
		}
		 
		public static string ReplaceStringLiterals(string text, MatchEvaluator eval)
		{
			return StringUtils.StringsRegex.Replace(text, eval);
		}
		 
		public static string Left(this string s, int length)
		{
			if (s.Length <= length)
			{
				return s;
			}
			return s.Substring(0, length);
		}
		 
		public static string LeftCut(this string s, int length)
		{
			return s.Substring(length);
		}
		 
		public static string Right(this string s, int length)
		{
			if (s.Length <= length)
			{
				return s;
			}
			return s.Substring(s.Length - length, length);
		}
		 
		public static string RightCut(this string s, int cutLength)
		{
			return s.Substring(0, s.Length - cutLength);
		}
		 
		[DebuggerNonUserCode]
		public static bool IsEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}
		 
		public static string FixName(this string name, bool extended)
		{
			return name.FixName(extended, -1);
		}
		 
		public static string FixName(this string name, bool extended, int nameLimit)
		{
			if (!extended && StringUtils.invalidPrefixRegex == null)
			{
				StringUtils.invalidPrefixRegex = new Regex("^[0-9_]+", RegexOptions.Compiled);
			}
			string text = (name == null) ? string.Empty : (extended ? name.Trim() : StringUtils.invalidPrefixRegex.Replace(name.WithoutInvalidChars(), string.Empty));
			if (nameLimit != -1 && text.Length > nameLimit)
			{
				text = text.Left(nameLimit);
			}
			return text;
		}
		 
		public static string FixFileName(this string fileName)
		{
			if (StringUtils.invalidFileNameCharsRegex == null)
			{
				StringUtils.invalidFileNameCharsRegex = new Regex("[" + Regex.Escape(Path.GetInvalidFileNameChars().StrCat("")) + "]", RegexOptions.Compiled);
			}
			if (fileName != null)
			{
				return StringUtils.invalidFileNameCharsRegex.Replace(fileName, "_");
			}
			return string.Empty;
		}
		 
		public static string ToPascalCase(this string value)
		{
			string text = value.Trim();
			if (text.Length == 0)
			{
				return value;
			}
			string[] array;
			if (text.IndexOf('_') > -1)
			{
				array = text.Split(new char[]
				{
					'_'
				});
			}
			else if (text.IndexOf(' ') > -1)
			{
				array = text.Split(new char[]
				{
					' '
				});
			}
			else if (text.IndexOf('-') > -1)
			{
				array = text.Split(new char[]
				{
					'-'
				});
			}
			else
			{
				array = new string[]
				{
					text
				};
			}
			text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Trim().Length > 0)
				{
					text = text + array[i].Substring(0, 1).ToUpper() + array[i].Substring(1);
				}
			}
			return text;
		}
		 
		public static string ToLabel(this string name)
		{
			if (name.IsEmpty())
			{
				return name;
			}
			string text = name.ToLabelAllowingIdSuffix();
			if (text.EndsWith(" id", StringComparison.OrdinalIgnoreCase))
			{
				text = text.Substring(0, text.Length - 3);
			}
			return text;
		}

 		public static string ToLabelAllowingIdSuffix(this string name)
		{
			string text = name.Replace('_', ' ');
			for (int i = 0; i < text.Length - 1; i++)
			{
				if ((char.IsLower(text[i]) && char.IsUpper(text[i + 1]) && !text.Substring(0, i + 1).IsOneOf(new string[]
				{
					"e",
					"i"
				})) || (i + 2 < text.Length && char.IsUpper(text[i]) && char.IsUpper(text[i + 1]) && char.IsLower(text[i + 2])) || (!char.IsDigit(text[i]) && char.IsDigit(text[i + 1])))
				{
					text = text.Substring(0, i + 1) + " " + text.Substring(i + 1);
					i++;
				}
			}
			return text;
		}

 		public static string ToCSSValue(this string cssClass)
		{
			return Regex.Replace(cssClass, "[a-z][A-Z]", (Match m) => m.Value[0].ToString() + "-" + m.Value[1].ToString()).ToLowerInvariant();
		}

 		public static string F(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

 		public static string[] SafeSplit(this string s, char separator)
		{
			if (!s.IsEmpty())
			{
				return s.Split(new char[]
				{
					separator
				});
			}
			return EmptyArray<string>.Instance;
		}

 		public static string NormalizeNewLines(this string s)
		{
			return s.Replace("\r", null);
		}

 		public static string RemoveNewLines(this string s)
		{
			s = s.Replace("\r", null);
			while (s.Contains("\n\n"))
			{
				s = s.Replace("\n\n", "\n");
			}
			s = s.Replace("\n", " ");
			return s;
		}

 		public static string RemoveIfStartsWith(this string s, string toRemove)
		{
			if (s.StartsWith(toRemove))
			{
				return s.Substring(toRemove.Length);
			}
			return s;
		}

 		public static string RemoveIfEndsWith(this string s, string toRemove)
		{
			if (s.EndsWith(toRemove))
			{
				return s.Substring(0, s.Length - toRemove.Length);
			}
			return s;
		}

 		public static string WithoutTrailingDigits(this string s)
		{
			return s.TrimEnd(new char[]
			{
				'0',
				'1',
				'2',
				'3',
				'4',
				'5',
				'6',
				'7',
				'8',
				'9'
			});
		}

 		public static string WithoutWhiteSpace(this string s)
		{
			return s.Replace(" ", null).Replace("\n", null).Replace("\r", null).Replace("\t", null);
		}

 		public static string TrimStart(this string s)
		{
			int num = 0;
			while (num < s.Length && char.IsWhiteSpace(s[num]))
			{
				num++;
			}
			return s.Substring(num);
		}

 		public static string TrimEnd(this string s)
		{
			int num = s.Length - 1;
			while (num >= 0 && char.IsWhiteSpace(s[num]))
			{
				num--;
			}
			return s.Substring(0, num);
		}

 		public static string Capitalize(this string s)
		{
			if (s.IsEmpty())
			{
				return s;
			}
			return s[0].ToString().ToUpper() + s.Substring(1);
		}

 		public static string DecapitalizeFirstWord(this string s)
		{
			if (s.IsEmpty())
			{
				return s;
			}
			if (char.IsLower(s[0]))
			{
				throw new InvalidOperationException("String '" + s + "' is not in the correct case");
			}
			string text = string.Empty;
			int num = 0;
			while (num < s.Length && char.IsUpper(s[num]))
			{
				text += char.ToLower(s[num++]).ToString();
			}
			if (num == 1)
			{
				return text + s.Substring(num);
			}
			if (num == s.Length)
			{
				return text;
			}
			return text.Substring(0, num - 1) + s.Substring(num - 1);
		}

 		public static bool EqualsIgnoreCase(this string str, string other)
		{
			return (str ?? string.Empty).Equals(other ?? string.Empty, StringComparison.OrdinalIgnoreCase);
		}

 		public static bool EndsWithIgnoreCase(this string str, string other)
		{
			return (str ?? string.Empty).EndsWith(other ?? string.Empty, StringComparison.OrdinalIgnoreCase);
		}

 		public static int IndexOfIgnoreCase(this string str, string subStr)
		{
			return str.IndexOf(subStr, StringComparison.OrdinalIgnoreCase);
		}

 		public static int IndexOfIgnoreCase(this string str, string subStr, int startIndex)
		{
			return str.IndexOf(subStr, startIndex, StringComparison.OrdinalIgnoreCase);
		}

 		public static int CountOccurrences(this string str, string subStr)
		{
			int startIndex = 0;
			int num = 0;
			int num2;
			while ((num2 = str.IndexOf(subStr, startIndex)) != -1)
			{
				num++;
				startIndex = num2 + subStr.Length;
			}
			return num;
		}

 		public static int CountOccurrencesIgnoringCase(this string str, string subStr)
		{
			int startIndex = 0;
			int num = 0;
			int num2;
			while ((num2 = str.IndexOfIgnoreCase(subStr, startIndex)) != -1)
			{
				num++;
				startIndex = num2 + subStr.Length;
			}
			return num;
		}

 		public static string FixEmptyStringAndNormalizeNewLines(this string str)
		{
			if (!str.IsEmpty())
			{
				return str.NormalizeNewLines();
			}
			return null;
		}

 		public static string FixEmptyStringAndRemoveNewLines(this string str)
		{
			if (!str.IsEmpty())
			{
				return str.RemoveNewLines();
			}
			return null;
		}

 		public static string EscapeXml(this string s)
		{
			if (s != null)
			{
				s = s.Replace("&", "&amp;");
				s = s.Replace("<", "&lt;");
				s = s.Replace(">", "&gt;");
				s = s.Replace("\"", "&quot;");
			}
			return s;
		}

 		public static string UnescapeXml(this string s)
		{
			if (s != null)
			{
				s = s.Replace("&lt;", "<");
				s = s.Replace("&gt;", ">");
				s = s.Replace("&quot;", "\"");
				s = s.Replace("&amp;", "&");
			}
			return s;
		}

 		public static string FixHtmlSpaces(this string s)
		{
			if (s.IsEmpty())
			{
				return s;
			}
			char[] array = new char[s.Length];
			if (s[0] == ' ')
			{
				array[0] = '\u00a0';
			}
			else
			{
				array[0] = s[0];
			}
			for (int i = 1; i < s.Length - 1; i++)
			{
				if (s[i] == ' ' && (s[i - 1] == '\n' || s[i + 1] == ' '))
				{
					array[i] = '\u00a0';
				}
				else
				{
					array[i] = s[i];
				}
			}
			if (s.Length != 1)
			{
				if (s[s.Length - 1] == ' ')
				{
					array[s.Length - 1] = '\u00a0';
				}
				else
				{
					array[s.Length - 1] = s[s.Length - 1];
				}
			}
			return new string(array);
		}

 		public static string EscapeStringLiteral(this string s)
		{
			return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
		}

 		public static string SafeEscapeString(this string s)
		{
			return s.Replace("\\\t", "\\t").Replace("\t", "\\t").Replace("\\\r", "\\r").Replace("\r", "\\r").Replace("\\\n", "\\n").Replace("\n", "\\n");
		}

 		public static string ToSourceCode(this string s)
		{
			if (s == null)
			{
				return "null";
			}
			return "\"" + s.EscapeStringLiteral() + "\"";
		}

 		public static string PrefixIfNotEmpty(this string s, string prefix)
		{
			if (!s.IsEmpty())
			{
				return prefix + s;
			}
			return s;
		}

 		public static string SurroundedInQuotes(this string s)
		{
			return s.SurroundedInQuotes('"');
		}

 		public static string SurroundedInQuotes(this string s, char quoteChar)
		{
			return quoteChar.ToString() + s.WithoutSurroundingQuotes(new char[]
			{
				quoteChar
			}) + quoteChar.ToString();
		}

 		public static string WithoutSurroundingQuotes(this string s)
		{
			return s.WithoutSurroundingQuotes(new char[]
			{
				'"'
			});
		}

 		public static string WithoutSurroundingQuotes(this string s, params char[] quoteChars)
		{
			if (s.Length > 1)
			{
				foreach (char c in quoteChars)
				{
					if (s.StartsWith(c.ToString()) && s.EndsWith(c.ToString()))
					{
						return s.Substring(1, s.Length - 2);
					}
				}
			}
			return s;
		}

 		public static bool StartsWithSegment(this string str, string segment)
		{
			return str.EqualsIgnoreCase(segment) || ((str.StartsWith(segment) || str.StartsWith(segment.ToLower())) && (char.IsUpper(str[segment.Length]) || !char.IsLetter(str[segment.Length]))) || (str.StartsWith(segment.ToUpper()) && (char.IsLower(str[segment.Length]) || !char.IsLetter(str[segment.Length])));
		}

 		public static bool EndsWithSegment(this string str, string segment)
		{
			return str.EqualsIgnoreCase(segment) || ((str.EndsWith(segment) || str.EndsWith(segment.ToLower())) && (char.IsUpper(str[str.Length - segment.Length]) || !char.IsLetter(str[str.Length - segment.Length]))) || (str.EndsWith(segment.ToUpper()) && (char.IsLower(str[str.Length - segment.Length]) || !char.IsLetter(str[str.Length - segment.Length])));
		}

 		public static bool ContainsSegment(this string str, string segment)
		{
			if (str.StartsWithSegment(segment))
			{
				return true;
			}
			int num = str.IndexOfIgnoreCase(segment);
			if (num <= 0)
			{
				return false;
			}
			if (char.IsLetter(str[num - 1]) && char.IsLower(str[num - 1]) == char.IsLower(str[num]))
			{
				return false;
			}
			if (str.Length > num + segment.Length)
			{
				if (char.IsLetter(str[num + 1]) && char.IsLower(str[num + 1]) == char.IsLower(str[num]))
				{
					return false;
				}
				if (char.IsLetter(str[num + segment.Length]) && char.IsLower(str[num + segment.Length]))
				{
					return false;
				}
			}
			return true;
		}

 		public static StringWriter CreateUtf8StringWriter()
		{
			return new StringUtils.CustomEncodingStringWriter(Encoding.UTF8);
		}

 		public static StringWriter CreateStringWriter(Encoding encoding)
		{
			return new StringUtils.CustomEncodingStringWriter(encoding);
		}

 		public static string TrimWithEllipsis(this string s, int length)
		{
			if (s.Length > length && length + 3 <= s.Length)
			{
				return s.Substring(0, length) + "...";
			}
			return s;
		}

 		public static string GetStringWithoutBOM(this Encoding encoding, byte[] strBytes)
		{
			string result;
			using (StreamReader streamReader = new StreamReader(new MemoryStream(strBytes), encoding))
			{
				result = streamReader.ReadToEnd();
			}
			return result;
		}

 		public static string Repeat(this string s, int n)
		{
			return new string(Enumerable.Range(0, n).SelectMany((int x) => s).ToArray<char>());
		}

 		private static int SurrogatePairCount(this string t, int endIndex)
		{
			int num = 0;
			int num2 = 0;
			while (num2 < endIndex && num2 + num < t.Length)
			{
				if (char.IsHighSurrogate(t, num2 + num))
				{
					num++;
				}
				num2++;
			}
			return num;
		}

 		private static int Index(this string t, string s, int startIndex, StringComparison comparisonType, bool searchFromEnd)
		{
			int result;
			try
			{
				if (s.Length == 0 || startIndex < 0 || startIndex > t.Length)
				{
					result = -1;
				}
				else
				{
					if (startIndex > 0)
					{
						startIndex += t.SurrogatePairCount(startIndex);
					}
					int num = searchFromEnd ? t.LastIndexOf(s, (startIndex > 0) ? startIndex : t.Length, comparisonType) : t.IndexOf(s, (startIndex > 0) ? startIndex : 0, comparisonType);
					if (num < 0)
					{
						result = -1;
					}
					else
					{
						result = t.Substring(0, num).LengthWithSC();
					}
				}
			}
			catch
			{
				result = -1;
			}
			return result;
		}

 		public static int IndexOfWithSC(this string t, string s, int startIndex, StringComparison comparisonType)
		{
			return t.Index(s, startIndex, comparisonType, false);
		}

 		public static int IndexOfWithSC(this string str, string value, StringComparison comparisonType)
		{
			return str.IndexOfWithSC(value, 0, comparisonType);
		}

 		public static int LastIndexOfWithSC(this string str, string value, int startIndex, StringComparison comparisonType)
		{
			return str.Index(value, startIndex, comparisonType, true);
		}

 		public static int LastIndexOfWithSC(this string str, string value, StringComparison comparisonType)
		{
			return str.LastIndexOfWithSC(value, str.LengthWithSC() - 1, comparisonType);
		}

 		public static string SubstringWithSC(this string str, int startIndex, int length)
		{
			if (str.IsNullOrEmpty())
			{
				return string.Empty;
			}
			return new StringInfo(str).SubstringByTextElements(startIndex, length);
		}

 		public static int LengthWithSC(this string str)
		{
			return new StringInfo(str).LengthInTextElements;
		}

 		public static string JoinText(string separator, params string[] parts)
		{
			return string.Join(separator, from p in parts
										  where !p.IsEmpty()
										  select p);
		}

 		public static int IndexOf(this string str, Regex regex, int startIndex = 0)
		{
			MatchCollection matchCollection = regex.Matches(str ?? string.Empty, startIndex);
			if (matchCollection.Count == 0)
			{
				return -1;
			}
			return matchCollection[0].Index;
		}

 		private static readonly HashSet<char> CharsToRemoveInsteadOfReplacing = new HashSet<char>
		{
			' ',
			'-',
			'&'
		};
		 
		private static Regex blockCommentsRegex;
		 
		private static Regex lineCommentsRegex;
		 
		private static Regex stringsRegex;
		 
		private static string validQueryForCountPattern = "^(\\s|\\()*SELECT\\b";
		 
		private static RegexOptions commonOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled;
		 
		private static Regex validQueryForCountSingleLineRegex;
		 
		private static Regex validQueryForCountMultiLineRegex;
		 
		private static Regex invalidPrefixRegex;
		 
		private static Regex invalidFileNameCharsRegex;
		 
		private sealed class CustomEncodingStringWriter : StringWriter
		{ 
			public CustomEncodingStringWriter(Encoding encoding)
			{
				this.encoding = encoding;
			}
			 
			public override Encoding Encoding
			{
				get
				{
					return this.encoding;
				}
			}
			 
			private readonly Encoding encoding;
		}
	}
}
