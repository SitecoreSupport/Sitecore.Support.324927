namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.DatasourceUsages
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.ExperienceEditor.Utils;
  using Sitecore.Links;

  public class GetDatasourceUsagesWithLockedStatus : Sitecore.ExperienceEditor.Speak.Ribbon.Requests.DatasourceUsages.GetDatasourceUsagesWithLockedStatus
  {
    protected override ItemLink[] GetItemLinks(Item item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      var layoutField = new LayoutField(WebUtility.IsEditAllVersionsTicked() ? item.Fields[FieldIDs.LayoutField] : item.Fields[FieldIDs.FinalLayoutField]);
      LinksValidationResult finalLinksResult = new LinksValidationResult(layoutField.InnerField, ItemLinkState.Valid);
      //validation of the Links fills out the Links of the result;
      layoutField.ValidateLinks(finalLinksResult);
      return finalLinksResult.Links.ToArray();
    }
  }
}