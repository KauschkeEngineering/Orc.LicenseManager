﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicensePoco.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LicensePoco : IDates, ICreator
    {
        #region Properties
        [Key]
        public int Id { get; set; }

        public string Value { get; set; }
        public string ExpireVersion { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpireDate { get; set; }
        public int CustomerId { get; set; }
        

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        #endregion

        #region ICreator Members
        [ScaffoldColumn(false)]
        public string CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        #endregion

        #region IDates Members
        [ScaffoldColumn(false)]
        public DateTime ModificationDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }
        #endregion
    }
}