
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
    
public partial class Dimensions
{

    public Dimensions()
    {

        this.ProfilesDimensions = new HashSet<ProfilesDimensions>();

        this.UsersDimensions = new HashSet<UsersDimensions>();

    }


    public int IdDimension { get; set; }

    public string Description { get; set; }

    public Nullable<int> IdDimensionType { get; set; }

    public Nullable<int> IdDimensionCategory { get; set; }

    public string Unit { get; set; }

    public Nullable<int> DurationHours { get; set; }

    public string LongDescription { get; set; }

    public Nullable<decimal> Value { get; set; }

    public Nullable<bool> SwitchValue { get; set; }

    public Nullable<bool> Active { get; set; }

    public string TagName { get; set; }

    public Nullable<bool> IsInfiniteByDefault { get; set; }



    public virtual DimensionsCategories DimensionsCategories { get; set; }

    public virtual DimensionsTypes DimensionsTypes { get; set; }

    public virtual ICollection<ProfilesDimensions> ProfilesDimensions { get; set; }

    public virtual ICollection<UsersDimensions> UsersDimensions { get; set; }

}

}
