using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint;

public partial class Item : ICustomMapping
{
    [CustomMapping(Columns = new String[] { "Modified", "Created", "Editor", "Author" })]
    public void MapFrom(object listItem)
    {
        SPListItem item = (SPListItem)listItem;
        this.ListItem = item;
        this.Modified = (DateTime)item["Modified"];
        this.Created = (DateTime)item["Created"];
        this.CreatedBy = (string)item["Author"];
        this.ModifiedBy = (string)item["Editor"];
    }

    public void MapTo(object listItem)
    {
        SPListItem item = (SPListItem)listItem;
        item["Modified"] = this.Modified;
        item["Created"] = this.Created;
        item["Author"] = this.CreatedBy;
        item["Editor"] = this.ModifiedBy;
    }

    public void Resolve(RefreshMode mode, object originalListItem, object databaseObject)
    {
        SPListItem originalItem = (SPListItem)originalListItem;
        SPListItem databaseItem = (SPListItem)databaseObject;

        DateTime originalModifiedValue = (DateTime)originalItem["Modified"];
        DateTime dbModifiedValue = (DateTime)databaseItem["Modified"];

        DateTime originalCreatedValue = (DateTime)originalItem["Created"];
        DateTime dbCreatedValue = (DateTime)databaseItem["Created"];

        string originalCreatedByValue = (string)originalItem["Author"];
        string dbCreatedByValue = (string)databaseItem["Author"];

        string originalModifiedByValue = (string)originalItem["Editor"];
        string dbModifiedByValue = (string)databaseItem["Editor"];

        if (mode == RefreshMode.OverwriteCurrentValues)
        {
            this.Modified = dbModifiedValue;
            this.Created = dbCreatedValue;
            this.CreatedBy = dbCreatedByValue;
            this.ModifiedBy = dbModifiedByValue;
        }
        else if (mode == RefreshMode.KeepCurrentValues)
        {
            databaseItem["Modified"] = this.Modified;
            databaseItem["Created"] = this.Created;
            databaseItem["Author"] = this.CreatedBy;
            databaseItem["Editor"] = this.ModifiedBy;
        }
        else if (mode == RefreshMode.KeepChanges)
        {
            if (this.Modified != originalModifiedValue)
            {
                databaseItem["Modified"] = this.Modified;
            }
            else if (this.Modified == originalModifiedValue && this.Modified != dbModifiedValue)
            {
                this.Modified = dbModifiedValue;
            }

            if (this.Created != originalCreatedValue)
            {
                databaseItem["Created"] = this.Created;
            }
            else if (this.Created == originalCreatedValue && this.Created != dbCreatedValue)
            {
                this.Created = dbCreatedValue;
            }

            if (this.CreatedBy != originalCreatedByValue)
            {
                databaseItem["Author"] = this.CreatedBy;
            }
            else if (this.CreatedBy == originalCreatedByValue && this.CreatedBy != dbCreatedByValue)
            {
                this.CreatedBy = dbCreatedByValue;
            }

            if (this.ModifiedBy != originalModifiedByValue)
            {
                databaseItem["Editor"] = this.ModifiedBy;
            }
            else if (this.ModifiedBy == originalModifiedByValue && this.ModifiedBy != dbModifiedByValue)
            {
                this.ModifiedBy = dbModifiedByValue;
            }
        }
    }


    public SPListItem ListItem { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }

    private List<Attachment> _attachments;
    public List<Attachment> Attachments
    {
        get
        {
            return _attachments;
        }
        private set
        {
            _attachments = value;
        }
    }

    public void AddAttachment(string fileName, byte[] dataBytes)
    {
        if (_attachments == null)
            _attachments = new List<Attachment>();

        Attachments.Add(new Attachment { FileName = fileName, DataBytes = dataBytes });
    }

    public override string ToString()
    {
        return _title;
    }
}

public class Attachment
{
    public byte[] DataBytes { get; set; }
    public string FileName { get; set; }
}
