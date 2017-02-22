namespace GeekLearning.AspNetCore.Mvc.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement(Attributes = "asp-readonly")]
    public class ReadonlyTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-readonly")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("asp-readonly");

            if (this.Condition)
            {
                output.Attributes.Add("readonly", "readonly");
            }
        }
    }
}
