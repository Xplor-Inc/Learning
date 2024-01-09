﻿namespace XploringMe.Core.Models.Entities.Enquiries;
public class Enquiry : Auditable
{
    public new long?        CreatedById         { get; set; }
    public string           Email               { get; set; } = string.Empty;
    public bool             IsResolved          { get; set; }
    public string           Message             { get; set; } = string.Empty;
    public string           Name                { get; set; } = string.Empty;
    public string?          Resolution          { get; set; }
    public string?          VisitorId           { get; set; }
}