﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InfConstractions.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.Entity.Infrastructure;

    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
        public Entities(EntityConnection Conn):base(Conn,false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActualStatu> ActualStatus { get; set; }
        public virtual DbSet<AddressObjectType> AddressObjectTypes { get; set; }
        public virtual DbSet<CenterStatu> CenterStatus { get; set; }
        public virtual DbSet<CurrentStatu> CurrentStatus { get; set; }
        public virtual DbSet<EstateStatu> EstateStatus { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<HouseStateStatu> HouseStateStatus { get; set; }
        public virtual DbSet<Object> Objects { get; set; }
        public virtual DbSet<OperationStatu> OperationStatus { get; set; }
        public virtual DbSet<StructureStatu> StructureStatus { get; set; }
    }
}
