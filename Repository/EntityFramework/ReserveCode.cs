
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Repository.EntityFramework
{

using System;
    using System.Collections.Generic;
    
public partial class ReserveCode
{

    public int IdReserveCode { get; set; }

    public Nullable<int> IdUser { get; set; }

    public int IdProduct { get; set; }

    public int IdState { get; set; }

    public string Value { get; set; }

    public string Ani { get; set; }

    public string Code { get; set; }

    public Nullable<System.DateTime> ExpirationDate { get; set; }

    public Nullable<System.DateTime> ModificationDate { get; set; }

    public System.DateTime CreationDate { get; set; }



    public virtual Products Products { get; set; }

    public virtual State State { get; set; }

    public virtual Users Users { get; set; }

}

}
