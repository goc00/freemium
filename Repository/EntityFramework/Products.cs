
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
    
public partial class Products
{

    public Products()
    {

        this.DimensionsCategories = new HashSet<DimensionsCategories>();

        this.ProductsForms = new HashSet<ProductsForms>();

        this.Profiles = new HashSet<Profiles>();

        this.Users = new HashSet<Users>();

        this.ReserveCode = new HashSet<ReserveCode>();

    }


    public int IdProduct { get; set; }

    public string Description { get; set; }

    public Nullable<int> IdPublisher { get; set; }

    public string CallbackUrlBilling { get; set; }

    public string Token { get; set; }

    public string TagName { get; set; }

    public Nullable<bool> DemoMode { get; set; }

    public string BillingUrlOk { get; set; }

    public string BillingUrlError { get; set; }

    public string BillingUrlProduct { get; set; }

    public string CodeAnalytics { get; set; }



    public virtual ICollection<DimensionsCategories> DimensionsCategories { get; set; }

    public virtual Publishers Publishers { get; set; }

    public virtual ICollection<ProductsForms> ProductsForms { get; set; }

    public virtual ICollection<Profiles> Profiles { get; set; }

    public virtual ICollection<Users> Users { get; set; }

    public virtual ICollection<ReserveCode> ReserveCode { get; set; }

}

}
