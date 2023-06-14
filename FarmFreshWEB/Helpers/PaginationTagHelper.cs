using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FarmFreshWEB.Helpers
{
	public class PaginationTagHelper:TagHelper
	{
		public override void Process(TagHelperContext context,TagHelperOutput output)
		{
			output.TagName = "nav";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.Add("aria-label", "Page navigation");
			output.Content.SetHtmlContent(AddPageContent());
		}

		public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageRange { get; set; }
        public int PageFirst { get; set; }
        public int PageLast { get; set; }
        public int PageTarget { get; set; }	

		private string AddPageContent()
		{
			if(PageRange == 0)
			{
				PageRange = 1;

			}
		}
    }
}
