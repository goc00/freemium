﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class FriPriEntities : DbContext
{
    public FriPriEntities()
        : base("name=FriPriEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<Configuration> Configuration { get; set; }

    public DbSet<Dimensions> Dimensions { get; set; }

    public DbSet<DimensionsCategories> DimensionsCategories { get; set; }

    public DbSet<DimensionsTypes> DimensionsTypes { get; set; }

    public DbSet<EventsLogs> EventsLogs { get; set; }

    public DbSet<Forms> Forms { get; set; }

    public DbSet<Products> Products { get; set; }

    public DbSet<ProductsForms> ProductsForms { get; set; }

    public DbSet<Profiles> Profiles { get; set; }

    public DbSet<ProfilesDimensions> ProfilesDimensions { get; set; }

    public DbSet<Publishers> Publishers { get; set; }

    public DbSet<Subscriptions> Subscriptions { get; set; }

    public DbSet<sysdiagrams> sysdiagrams { get; set; }

    public DbSet<Users> Users { get; set; }

    public DbSet<UsersDimensions> UsersDimensions { get; set; }

    public DbSet<Promos> Promos { get; set; }

    public DbSet<NaranyaNotification> NaranyaNotification { get; set; }

    public DbSet<Unsubscriptions> Unsubscriptions { get; set; }

    public DbSet<ReserveCode> ReserveCode { get; set; }

    public DbSet<State> State { get; set; }

}

}
