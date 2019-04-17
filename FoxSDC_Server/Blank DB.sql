﻿SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Config]( 
    [Key] [nvarchar](100) NOT NULL, 
    [Value] [nvarchar](1000) NOT NULL, 
CONSTRAINT [PK_Config] PRIMARY KEY CLUSTERED  
( 
   [Key] ASC 
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] 
) ON [PRIMARY] 
GO 
INSERT [Config] ([Key], [Value]) VALUES (N'ID', N'FOXSDCv1')
INSERT [Config] ([Key], [Value]) VALUES (N'Version', N'0')

