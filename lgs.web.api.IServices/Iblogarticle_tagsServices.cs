using lgs.web.api.IServices.BASE;
using lgs.web.api.Model.Models;
using System.Collections.Generic;

namespace lgs.web.api.IServices
{	
	/// <summary>
	/// Iblogarticle_tagsServices
	/// </summary>	
    public interface Iblogarticle_tagsServices :IBaseServices<blogarticle_tags>
	{
		List<string> GetTags(int page = 1, int intPageSize = 50);

	}
}