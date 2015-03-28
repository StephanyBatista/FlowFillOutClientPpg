using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class ClientRequestItem
{
    public string ApproverObservation { get; set; }
    public bool IsReturn { get; set; }

    public void SaveAttachments(SPWeb web)
    {
        if (Attachments == null || Attachments.Count == 0)
            return;

        var list = web.Lists["Solicita Cliente"];
        var item = list.GetItemById(Id.Value);

        foreach (var attachment in Attachments)
        {
            for (int i = 0; i < item.Attachments.Count; i++)
                if (item.Attachments[i].Contains(attachment.FileName))
                    item.Attachments.Delete(attachment.FileName);

            item.Attachments.AddNow(attachment.FileName, attachment.DataBytes);
        }

        item.Update();
        list.Update();
    }
}
