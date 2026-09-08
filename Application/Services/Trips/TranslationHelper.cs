using Application.DTOs.Common;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Trips
{
    internal static class TranslationHelper
    {
        private static readonly Language[] OptionalLanguages = { Language.German, Language.French, Language.Russian };

        /// <summary>
        /// Determines if an optional language has any content in the provided LocalizedTextDto fields.
        /// </summary>
        public static bool HasContent(Language language, params LocalizedTextDto?[] fields)
        {
            if (language == Language.English)
                throw new ArgumentException("English is mandatory and should not be checked with HasContent.");

            return fields.Any(field => field != null && !string.IsNullOrWhiteSpace(GetLanguageValue(field, language)));
        }

        /// <summary>
        /// Gets the value for a specific language from LocalizedTextDto, or null if not found.
        /// </summary>
        public static string? GetLanguageValue(LocalizedTextDto? dto, Language language)
        {
            if (dto == null) return null;

            return language switch
            {
                Language.English => dto.English,
                Language.German => dto.German,
                Language.French => dto.French,
                Language.Russian => dto.Russian,
                _ => null
            };
        }

        /// <summary>
        /// Gets the trimmed value for a language, falling back to English if not provided.
        /// </summary>
        public static string GetValueOrEnglishFallback(LocalizedTextDto dto, Language language, bool isRequired = false)
        {
            var value = GetLanguageValue(dto, language);
            var englishValue = dto.English;

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            if (!string.IsNullOrWhiteSpace(englishValue))
                return englishValue.Trim();

            return isRequired ? string.Empty : string.Empty;
        }

        /// <summary>
        /// Gets the trimmed value for a language, falling back to English. Returns null if both are empty.
        /// </summary>
        public static string? GetOptionalValueOrEnglishFallback(LocalizedTextDto? dto, Language language)
        {
            if (dto == null) return null;

            var value = GetLanguageValue(dto, language);
            var englishValue = dto.English;

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            if (!string.IsNullOrWhiteSpace(englishValue))
                return englishValue.Trim();

            return null;
        }

        /// <summary>
        /// Returns all languages that should have translations created (English + optional languages with content).
        /// </summary>
        public static IEnumerable<Language> GetLanguagesToCreate(params LocalizedTextDto?[] fields)
        {
            // English is always included
            yield return Language.English;

            // Check each optional language
            foreach (var language in OptionalLanguages)
            {
                if (HasContent(language, fields))
                {
                    yield return language;
                }
            }
        }
    }
}
