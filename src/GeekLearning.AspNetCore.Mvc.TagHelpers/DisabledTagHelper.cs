namespace GeekLearning.AspNetCore.Mvc.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement(Attributes = "asp-disabled")]
    public class DisabledTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-disabled")]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("asp-disabled");

            if (this.Condition)
            {
                output.Attributes.Add("disabled", "disabled");
            }
        }
    }
}
