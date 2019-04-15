namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.DatasourceUsages
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.ExperienceEditor.Utils;
  using Sitecore.Links;
  using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
  using Sitecore.XA.Feature.Composites;
  using System.Collections.Generic;
  using System.Linq;

  public class GetDatasourceUsagesWithLockedStatus : Sitecore.ExperienceEditor.Speak.Ribbon.Requests.DatasourceUsages.GetDatasourceUsagesWithLockedStatus
  {
    protected override ItemLink[] GetItemLinks(Item item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      var layoutField = new LayoutField(WebUtility.IsEditAllVersionsTicked() ? item.Fields[FieldIDs.LayoutField] : item.Fields[FieldIDs.FinalLayoutField]);
      LinksValidationResult finalLinksResult = new LinksValidationResult(layoutField.InnerField, ItemLinkState.Valid);
      //validation of the Links fills out the Links of the result;
      layoutField.ValidateLinks(finalLinksResult);
      #region Added code
      List<ItemLink> compositeSources = new List<ItemLink>();
      foreach (ItemLink link in finalLinksResult.Links.Where(l => l.TargetItemID != item.ID))
      {
        Item linkedItem = Sitecore.Context.Database.GetItem(link.TargetItemID);
        if (linkedItem.InheritsFrom(Templates.CompositeGroup.ID)) // for regular Composite
        {
          foreach (Item child in linkedItem.Children)
          {
            compositeSources.Add(new ItemLink (linkedItem, Sitecore.FieldIDs.FinalLayoutField, child, child.Paths.Path));
            compositeSources.AddRange(this.GetItemLinks(child));
          }
        }
        else if (linkedItem.InheritsFrom(Templates.CompositeSection.ID)) //for Snippet
        {
          compositeSources.AddRange(this.GetItemLinks(linkedItem));
        }
      }
      finalLinksResult.Links.AddRange(compositeSources);
      #endregion
      return finalLinksResult.Links.ToArray();
    }
  }
}