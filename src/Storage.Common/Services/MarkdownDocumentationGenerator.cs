using System.Collections.Generic;
using System.Text;
using Microsoft.AspNet.Mvc.Description;

namespace Storage.Common.Services
{
    public class MarkdownDocumentationGenerator
    {
        public string CreateApiDocumentation(IEnumerable<ApiDescriptionGroup> groups)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("# Controllers");

            foreach (var group in groups)
            {
                stringBuilder.AppendLine($"## {group.GroupName}");
                stringBuilder.AppendLine("----");

                foreach (var description in group.Items)
                {
                    stringBuilder
                        .AppendLine($"### {description.ActionDescriptor.Name}")
                        .AppendLine($"    {description.HttpMethod} {description.RelativePath}");

                    if (description.ParameterDescriptions.Count > 0)
                    {
                        stringBuilder.AppendLine("#### Parameters");
                        foreach (var parameter in description.ParameterDescriptions)
                        {
                            stringBuilder
                                .AppendLine($"> {parameter.Name} ({parameter.Type.Name})")
                                .AppendLine();
                        }
                    }
                    if (description.ResponseType != null)
                    {
                        stringBuilder.AppendLine("#### Response");
                        if (description.ResponseType.IsGenericType)
                        {
                            var genericType = description.ResponseType.GetGenericArguments()[0];
                            stringBuilder
                                .AppendLine($"> {genericType.Name}[]");
                        }
                        else
                        {
                            stringBuilder
                                .AppendLine($"> {description.ResponseType.Name}");
                        }
                    }
                }
            }
            return stringBuilder.ToString();
        }
    }
}