namespace GeekLearning.AspNetCore.Semver
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    [HtmlTargetElement("semver", TagStructure = TagStructure.WithoutEndTag)]
    public class SemverTagHelper : TagHelper
    {
        private readonly IOptions<SemverOptions> options;

        public SemverTagHelper(IOptions<SemverOptions> options)
        {
            this.options = options;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetContent(this.options.Value.SemVer);
        }
    }
}
