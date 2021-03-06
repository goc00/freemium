
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
    
public partial class Profiles
{

    public Profiles()
    {

        this.ProfilesDimensions = new HashSet<ProfilesDimensions>();

        this.Subscriptions = new HashSet<Subscriptions>();

    }


    public int IdProfile { get; set; }

    public Nullable<int> IdProduct { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Nullable<decimal> PriceUSD { get; set; }

    public Nullable<bool> Active { get; set; }

    public string TagName { get; set; }

    public Nullable<bool> AnonDefault { get; set; }

    public Nullable<bool> UserDefault { get; set; }

    public Nullable<bool> Paid { get; set; }

    public Nullable<bool> Featured { get; set; }

    public string MotivatorText { get; set; }

    public string ShortDescription { get; set; }

    public string Country { get; set; }



    public virtual Products Products { get; set; }

    public virtual ICollection<ProfilesDimensions> ProfilesDimensions { get; set; }

    public virtual ICollection<Subscriptions> Subscriptions { get; set; }

}

}
