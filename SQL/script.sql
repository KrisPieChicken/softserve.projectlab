/****** Object:  Table [dbo].[BranchEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchEntity](
	[BranchId] [int] IDENTITY(1,1) NOT NULL,
	[BranchName] [varchar](255) NOT NULL,
	[BranchCity] [varchar](255) NOT NULL,
	[BranchAddress] [varchar](255) NOT NULL,
	[BranchRegion] [varchar](255) NOT NULL,
	[BranchContactNumber] [varchar](20) NOT NULL,
	[BranchContactEmail] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BusinessCustomerEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessCustomerEntity](
	[CustomerId] [int] NOT NULL,
	[CompanyName] [varchar](255) NOT NULL,
	[TaxId] [varchar](50) NOT NULL,
	[Industry] [varchar](100) NOT NULL,
	[EmployeeCount] [int] NOT NULL,
	[AnnualRevenue] [decimal](18, 2) NOT NULL,
	[BusinessSize] [varchar](20) NOT NULL,
	[VolumeDiscountRate] [decimal](5, 2) NOT NULL,
	[CreditTerms] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartEntity](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItemEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItemEntity](
	[CartId] [int] NOT NULL,
	[Sku] [int] NOT NULL,
	[ItemQuantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CartId] ASC,
	[Sku] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CatalogCategoryEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CatalogCategoryEntity](
	[CatalogId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[CategoryName] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CatalogId] ASC,
	[CategoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CatalogEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CatalogEntity](
	[CatalogId] [int] IDENTITY(1,1) NOT NULL,
	[CatalogName] [varchar](255) NOT NULL,
	[CatalogDescription] [varchar](max) NOT NULL,
	[CatalogStatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CatalogId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryEntity](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](255) NOT NULL,
	[CategoryStatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreditTransactionEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditTransactionEntity](
	[Id] [varchar](100) NOT NULL,
	[TransactionType] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Description] [varchar](255) NULL,
	[TransactionDate] [datetime] NOT NULL,
	[LineOfCreditId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerEntity](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerType] [varchar](50) NOT NULL,
	[CustomerName] [varchar](255) NOT NULL,
	[CustomerContactNumber] [varchar](255) NOT NULL,
	[CustomerContactEmail] [varchar](255) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[BirthDate] [date] NULL,
	[Email] [varchar](150) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Address] [varchar](255) NULL,
	[City] [varchar](100) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](20) NULL,
	[RegistrationDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IndividualCustomerEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IndividualCustomerEntity](
	[CustomerId] [int] NOT NULL,
	[IsEligibleForPromotions] [bit] NOT NULL,
	[CommunicationPreference] [varchar](20) NOT NULL,
	[LoyaltyPoints] [int] NOT NULL,
	[LastPurchaseDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemEntity](
	[Sku] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [varchar](255) NOT NULL,
	[ItemDescription] [varchar](max) NOT NULL,
	[OriginalStock] [int] NOT NULL,
	[CurrentStock] [int] NOT NULL,
	[ItemCurrency] [varchar](10) NOT NULL,
	[ItemUnitCost] [decimal](10, 2) NOT NULL,
	[ItemMarginGain] [decimal](10, 2) NOT NULL,
	[ItemDiscount] [decimal](10, 2) NULL,
	[ItemAdditionalTax] [decimal](10, 2) NULL,
	[ItemPrice] [decimal](10, 2) NOT NULL,
	[ItemStatus] [bit] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ItemImage] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Sku] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LineOfCreditEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LineOfCreditEntity](
	[CustomerId] [int] NOT NULL,
	[CreditLimit] [decimal](10, 2) NOT NULL,
	[CurrentBalance] [decimal](10, 2) NOT NULL,
	[Id] [varchar](100) NOT NULL,
	[Provider] [varchar](100) NOT NULL,
	[AnnualInterestRate] [decimal](10, 5) NOT NULL,
	[OpenDate] [datetime] NOT NULL,
	[NextPaymentDueDate] [datetime] NOT NULL,
	[MinimumPaymentAmount] [decimal](18, 2) NOT NULL,
	[CreditScore] [int] NOT NULL,
	[Status] [varchar](20) NOT NULL,
	[LastReviewDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderEntity](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[OrderTotalAmount] [decimal](10, 2) NOT NULL,
	[OrderStatus] [varchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItemEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItemEntity](
	[OrderId] [int] NOT NULL,
	[Sku] [int] NOT NULL,
	[ItemQuantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[Sku] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageEntity](
	[PackageId] [int] IDENTITY(1,1) NOT NULL,
	[PackageName] [varchar](255) NOT NULL,
	[Description] [varchar](max) NULL,
	[SaleDate] [datetime] NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[ContractId] [varchar](100) NULL,
	[ContractTermMonths] [int] NOT NULL,
	[ContractStartDate] [datetime] NOT NULL,
	[MonthlyFee] [decimal](18, 2) NOT NULL,
	[SetupFee] [decimal](18, 2) NOT NULL,
	[DiscountAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [varchar](50) NOT NULL,
	[ShippingAddress] [varchar](255) NULL,
	[TrackingNumber] [varchar](100) NULL,
	[EstimatedDeliveryDate] [datetime] NULL,
	[ActualDeliveryDate] [datetime] NULL,
	[IsRenewal] [bit] NOT NULL,
	[PreviousPackageId] [int] NULL,
	[CustomerId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageItemEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageItemEntity](
	[PackageId] [int] NOT NULL,
	[Sku] [int] NOT NULL,
	[ItemQuantity] [int] NOT NULL,
	[Notes] [varchar](max) NULL,
	[WarrantyMonths] [int] NULL,
	[SerialNumber] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[PackageId] ASC,
	[Sku] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackageNoteEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackageNoteEntity](
	[Id] [varchar](100) NOT NULL,
	[PackageId] [int] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Content] [varchar](max) NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionEntity](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[PermissionName] [varchar](100) NOT NULL,
	[PermissionDescription] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PremiumCustomerEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PremiumCustomerEntity](
	[CustomerId] [int] NOT NULL,
	[DiscountRate] [decimal](5, 2) NOT NULL,
	[MembershipStartDate] [datetime] NOT NULL,
	[MembershipExpiryDate] [datetime] NOT NULL,
	[TierLevel] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleEntity](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
	[RoleDescription] [varchar](max) NOT NULL,
	[RoleStatus] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissionEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissionEntity](
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
	[PermissionName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PermissionId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierEntity](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [varchar](255) NOT NULL,
	[SupplierAddress] [varchar](max) NOT NULL,
	[SupplierContactNumber] [varchar](20) NOT NULL,
	[SupplierContactEmail] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierItemEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierItemEntity](
	[SupplierId] [int] NOT NULL,
	[Sku] [int] NOT NULL,
	[ItemQuantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC,
	[Sku] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEntity](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserFirstName] [varchar](100) NOT NULL,
	[UserLastName] [varchar](100) NULL,
	[UserContactEmail] [varchar](255) NOT NULL,
	[UserContactNumber] [varchar](255) NOT NULL,
	[UserPassword] [varchar](255) NOT NULL,
	[UserStatus] [bit] NOT NULL,
	[BranchId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[UserContactEmail] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleEntity](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[RoleName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WarehouseEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseEntity](
	[WarehouseId] [int] IDENTITY(1,1) NOT NULL,
	[WarehouseLocation] [varchar](255) NOT NULL,
	[WarehouseCapacity] [int] NOT NULL,
	[BranchId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WarehouseId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WarehouseItemEntity]    Script Date: 07/04/2025 9:35:02 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseItemEntity](
	[WarehouseId] [int] NOT NULL,
	[Sku] [int] NOT NULL,
	[ItemQuantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WarehouseId] ASC,
	[Sku] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessCustomerEntity] ADD  DEFAULT ((1)) FOR [EmployeeCount]
GO
ALTER TABLE [dbo].[BusinessCustomerEntity] ADD  DEFAULT ((0.00)) FOR [AnnualRevenue]
GO
ALTER TABLE [dbo].[BusinessCustomerEntity] ADD  DEFAULT ('Small') FOR [BusinessSize]
GO
ALTER TABLE [dbo].[BusinessCustomerEntity] ADD  DEFAULT ((0.00)) FOR [VolumeDiscountRate]
GO
ALTER TABLE [dbo].[BusinessCustomerEntity] ADD  DEFAULT ('Net30') FOR [CreditTerms]
GO
ALTER TABLE [dbo].[CreditTransactionEntity] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[CreditTransactionEntity] ADD  DEFAULT (getutcdate()) FOR [TransactionDate]
GO
ALTER TABLE [dbo].[CustomerEntity] ADD  DEFAULT (getutcdate()) FOR [RegistrationDate]
GO
ALTER TABLE [dbo].[IndividualCustomerEntity] ADD  DEFAULT ((1)) FOR [IsEligibleForPromotions]
GO
ALTER TABLE [dbo].[IndividualCustomerEntity] ADD  DEFAULT ('Email') FOR [CommunicationPreference]
GO
ALTER TABLE [dbo].[IndividualCustomerEntity] ADD  DEFAULT ((0)) FOR [LoyaltyPoints]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT ('') FOR [Provider]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT ((0.00000)) FOR [AnnualInterestRate]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT (getutcdate()) FOR [OpenDate]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT (getutcdate()) FOR [NextPaymentDueDate]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT ((0.00)) FOR [MinimumPaymentAmount]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT ((0)) FOR [CreditScore]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT ('Active') FOR [Status]
GO
ALTER TABLE [dbo].[LineOfCreditEntity] ADD  DEFAULT (getutcdate()) FOR [LastReviewDate]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT (getutcdate()) FOR [SaleDate]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ('Processing') FOR [Status]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ((0)) FOR [ContractTermMonths]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT (getutcdate()) FOR [ContractStartDate]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ((0.00)) FOR [MonthlyFee]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ((0.00)) FOR [SetupFee]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ((0.00)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ('Credit Card') FOR [PaymentMethod]
GO
ALTER TABLE [dbo].[PackageEntity] ADD  DEFAULT ((0)) FOR [IsRenewal]
GO
ALTER TABLE [dbo].[PackageNoteEntity] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[PackageNoteEntity] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[PremiumCustomerEntity] ADD  DEFAULT ((0.00)) FOR [DiscountRate]
GO
ALTER TABLE [dbo].[PremiumCustomerEntity] ADD  DEFAULT (getutcdate()) FOR [MembershipStartDate]
GO
ALTER TABLE [dbo].[PremiumCustomerEntity] ADD  DEFAULT ('Silver') FOR [TierLevel]
GO
ALTER TABLE [dbo].[BusinessCustomerEntity]  WITH CHECK ADD  CONSTRAINT [FK_BusinessCustomer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BusinessCustomerEntity] CHECK CONSTRAINT [FK_BusinessCustomer_Customer]
GO
ALTER TABLE [dbo].[CartEntity]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
GO
ALTER TABLE [dbo].[CartEntity] CHECK CONSTRAINT [FK_Cart_Customer]
GO
ALTER TABLE [dbo].[CartItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Cart] FOREIGN KEY([CartId])
REFERENCES [dbo].[CartEntity] ([CartId])
GO
ALTER TABLE [dbo].[CartItemEntity] CHECK CONSTRAINT [FK_CartItem_Cart]
GO
ALTER TABLE [dbo].[CartItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_CartItem_Item] FOREIGN KEY([Sku])
REFERENCES [dbo].[ItemEntity] ([Sku])
GO
ALTER TABLE [dbo].[CartItemEntity] CHECK CONSTRAINT [FK_CartItem_Item]
GO
ALTER TABLE [dbo].[CatalogCategoryEntity]  WITH CHECK ADD  CONSTRAINT [FK_CatalogCategoryEntity_CatalogEntity] FOREIGN KEY([CatalogId])
REFERENCES [dbo].[CatalogEntity] ([CatalogId])
GO
ALTER TABLE [dbo].[CatalogCategoryEntity] CHECK CONSTRAINT [FK_CatalogCategoryEntity_CatalogEntity]
GO
ALTER TABLE [dbo].[CatalogCategoryEntity]  WITH CHECK ADD  CONSTRAINT [FK_CatalogCategoryEntity_CategoryEntity] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryEntity] ([CategoryId])
GO
ALTER TABLE [dbo].[CatalogCategoryEntity] CHECK CONSTRAINT [FK_CatalogCategoryEntity_CategoryEntity]
GO
ALTER TABLE [dbo].[CreditTransactionEntity]  WITH CHECK ADD  CONSTRAINT [FK_CreditTransaction_LineOfCredit] FOREIGN KEY([LineOfCreditId])
REFERENCES [dbo].[LineOfCreditEntity] ([CustomerId])
GO
ALTER TABLE [dbo].[CreditTransactionEntity] CHECK CONSTRAINT [FK_CreditTransaction_LineOfCredit]
GO
ALTER TABLE [dbo].[IndividualCustomerEntity]  WITH CHECK ADD  CONSTRAINT [FK_IndividualCustomer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[IndividualCustomerEntity] CHECK CONSTRAINT [FK_IndividualCustomer_Customer]
GO
ALTER TABLE [dbo].[ItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_Item_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[CategoryEntity] ([CategoryId])
GO
ALTER TABLE [dbo].[ItemEntity] CHECK CONSTRAINT [FK_Item_Category]
GO
ALTER TABLE [dbo].[LineOfCreditEntity]  WITH CHECK ADD  CONSTRAINT [FK_LineOfCredit_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
GO
ALTER TABLE [dbo].[LineOfCreditEntity] CHECK CONSTRAINT [FK_LineOfCredit_Customer]
GO
ALTER TABLE [dbo].[OrderEntity]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
GO
ALTER TABLE [dbo].[OrderEntity] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[OrderItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Item] FOREIGN KEY([Sku])
REFERENCES [dbo].[ItemEntity] ([Sku])
GO
ALTER TABLE [dbo].[OrderItemEntity] CHECK CONSTRAINT [FK_OrderItem_Item]
GO
ALTER TABLE [dbo].[OrderItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrderEntity] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItemEntity] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
ALTER TABLE [dbo].[PackageEntity]  WITH CHECK ADD  CONSTRAINT [FK_Package_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
GO
ALTER TABLE [dbo].[PackageEntity] CHECK CONSTRAINT [FK_Package_Customer]
GO
ALTER TABLE [dbo].[PackageItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_PackageItem_Item] FOREIGN KEY([Sku])
REFERENCES [dbo].[ItemEntity] ([Sku])
GO
ALTER TABLE [dbo].[PackageItemEntity] CHECK CONSTRAINT [FK_PackageItem_Item]
GO
ALTER TABLE [dbo].[PackageItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_PackageItem_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[PackageEntity] ([PackageId])
GO
ALTER TABLE [dbo].[PackageItemEntity] CHECK CONSTRAINT [FK_PackageItem_Package]
GO
ALTER TABLE [dbo].[PackageNoteEntity]  WITH CHECK ADD  CONSTRAINT [FK_PackageNote_Package] FOREIGN KEY([PackageId])
REFERENCES [dbo].[PackageEntity] ([PackageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PackageNoteEntity] CHECK CONSTRAINT [FK_PackageNote_Package]
GO
ALTER TABLE [dbo].[PremiumCustomerEntity]  WITH CHECK ADD  CONSTRAINT [FK_PremiumCustomer_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[CustomerEntity] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PremiumCustomerEntity] CHECK CONSTRAINT [FK_PremiumCustomer_Customer]
GO
ALTER TABLE [dbo].[RolePermissionEntity]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY([PermissionId])
REFERENCES [dbo].[PermissionEntity] ([PermissionId])
GO
ALTER TABLE [dbo].[RolePermissionEntity] CHECK CONSTRAINT [FK_RolePermission_Permission]
GO
ALTER TABLE [dbo].[RolePermissionEntity]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleEntity] ([RoleId])
GO
ALTER TABLE [dbo].[RolePermissionEntity] CHECK CONSTRAINT [FK_RolePermission_Role]
GO
ALTER TABLE [dbo].[SupplierItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_SupplierItem_Item] FOREIGN KEY([Sku])
REFERENCES [dbo].[ItemEntity] ([Sku])
GO
ALTER TABLE [dbo].[SupplierItemEntity] CHECK CONSTRAINT [FK_SupplierItem_Item]
GO
ALTER TABLE [dbo].[SupplierItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_SupplierItem_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[SupplierEntity] ([SupplierId])
GO
ALTER TABLE [dbo].[SupplierItemEntity] CHECK CONSTRAINT [FK_SupplierItem_Supplier]
GO
ALTER TABLE [dbo].[UserEntity]  WITH CHECK ADD  CONSTRAINT [FK_User_Branch] FOREIGN KEY([BranchId])
REFERENCES [dbo].[BranchEntity] ([BranchId])
GO
ALTER TABLE [dbo].[UserEntity] CHECK CONSTRAINT [FK_User_Branch]
GO
ALTER TABLE [dbo].[UserRoleEntity]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleEntity] ([RoleId])
GO
ALTER TABLE [dbo].[UserRoleEntity] CHECK CONSTRAINT [FK_UserRole_Role]
GO
ALTER TABLE [dbo].[UserRoleEntity]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserEntity] ([UserId])
GO
ALTER TABLE [dbo].[UserRoleEntity] CHECK CONSTRAINT [FK_UserRole_User]
GO
ALTER TABLE [dbo].[WarehouseEntity]  WITH CHECK ADD  CONSTRAINT [FK_Warehouse_Branch] FOREIGN KEY([BranchId])
REFERENCES [dbo].[BranchEntity] ([BranchId])
GO
ALTER TABLE [dbo].[WarehouseEntity] CHECK CONSTRAINT [FK_Warehouse_Branch]
GO
ALTER TABLE [dbo].[WarehouseItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseItem_Item] FOREIGN KEY([Sku])
REFERENCES [dbo].[ItemEntity] ([Sku])
GO
ALTER TABLE [dbo].[WarehouseItemEntity] CHECK CONSTRAINT [FK_WarehouseItem_Item]
GO
ALTER TABLE [dbo].[WarehouseItemEntity]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseItem_Warehouse] FOREIGN KEY([WarehouseId])
REFERENCES [dbo].[WarehouseEntity] ([WarehouseId])
GO
ALTER TABLE [dbo].[WarehouseItemEntity] CHECK CONSTRAINT [FK_WarehouseItem_Warehouse]
GO
