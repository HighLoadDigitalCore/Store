USE [master]
GO
/****** Object:  Database [boiler-gas.ru]    Script Date: 12.11.2018 13:44:49 ******/
CREATE DATABASE [boiler-gas.ru] ON  PRIMARY 
( NAME = N'stroy_data', FILENAME = N'D:\SQL2012R1\boiler-gas.ru.mdf' , SIZE = 982016KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'stroy_log', FILENAME = N'D:\SQL2012R1\boiler-gas.ru_log.ldf' , SIZE = 12288KB , MAXSIZE = 2048GB , FILEGROWTH = 1024KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [boiler-gas.ru].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [boiler-gas.ru] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET ARITHABORT OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [boiler-gas.ru] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [boiler-gas.ru] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [boiler-gas.ru] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET  DISABLE_BROKER 
GO
ALTER DATABASE [boiler-gas.ru] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [boiler-gas.ru] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [boiler-gas.ru] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [boiler-gas.ru] SET  MULTI_USER 
GO
ALTER DATABASE [boiler-gas.ru] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [boiler-gas.ru] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'boiler-gas.ru', N'ON'
GO
USE [boiler-gas.ru]
GO
/****** Object:  User [stroy]    Script Date: 12.11.2018 13:44:49 ******/
CREATE USER [stroy] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [aspnet_WebEvent_FullAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_WebEvent_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_ReportingAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Roles_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_FullAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Roles_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_BasicAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Roles_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_ReportingAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Profile_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_FullAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Profile_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_BasicAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Profile_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_ReportingAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_FullAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Personalization_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_BasicAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Personalization_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_ReportingAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Membership_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_FullAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Membership_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_BasicAccess]    Script Date: 12.11.2018 13:44:49 ******/
CREATE ROLE [aspnet_Membership_BasicAccess]
GO
sys.sp_addrolemember @rolename = N'db_owner', @membername = N'stroy'
GO
/****** Object:  Schema [aspnet_Membership_BasicAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Membership_BasicAccess]
GO
/****** Object:  Schema [aspnet_Membership_FullAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Membership_FullAccess]
GO
/****** Object:  Schema [aspnet_Membership_ReportingAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Membership_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Personalization_BasicAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Personalization_BasicAccess]
GO
/****** Object:  Schema [aspnet_Personalization_FullAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Personalization_FullAccess]
GO
/****** Object:  Schema [aspnet_Personalization_ReportingAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Profile_BasicAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Profile_BasicAccess]
GO
/****** Object:  Schema [aspnet_Profile_FullAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Profile_FullAccess]
GO
/****** Object:  Schema [aspnet_Profile_ReportingAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Profile_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Roles_BasicAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Roles_BasicAccess]
GO
/****** Object:  Schema [aspnet_Roles_FullAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Roles_FullAccess]
GO
/****** Object:  Schema [aspnet_Roles_ReportingAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_Roles_ReportingAccess]
GO
/****** Object:  Schema [aspnet_WebEvent_FullAccess]    Script Date: 12.11.2018 13:44:50 ******/
CREATE SCHEMA [aspnet_WebEvent_FullAccess]
GO
/****** Object:  FullTextCatalog [FullText_Products]    Script Date: 12.11.2018 13:44:50 ******/
CREATE FULLTEXT CATALOG [FullText_Products]WITH ACCENT_SENSITIVITY = OFF

GO
/****** Object:  FullTextCatalog [ProductSearch]    Script Date: 12.11.2018 13:44:50 ******/
CREATE FULLTEXT CATALOG [ProductSearch]WITH ACCENT_SENSITIVITY = ON
AS DEFAULT

GO
/****** Object:  StoredProcedure [dbo].[aspnet_AnyDataInTables]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check Membership table if (@TablesToCheck & 1) is set
    IF ((@TablesToCheck & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Membership))
        BEGIN
            SELECT N'aspnet_Membership'
            RETURN
        END
    END

    -- Check aspnet_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Roles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM dbo.aspnet_Roles))
        BEGIN
            SELECT N'aspnet_Roles'
            RETURN
        END
    END

    -- Check aspnet_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Profile))
        BEGIN
            SELECT N'aspnet_Profile'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_PersonalizationPerUser))
        BEGIN
            SELECT N'aspnet_PersonalizationPerUser'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'aspnet_WebEvent_LogEvent') AND (type = 'P'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM dbo.aspnet_WebEvent_Events))
        BEGIN
            SELECT N'aspnet_WebEvent_Events'
            RETURN
        END
    END

    -- Check aspnet_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Users))
        BEGIN
            SELECT N'aspnet_Users'
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM dbo.aspnet_Applications))
        BEGIN
            SELECT N'aspnet_Applications'
            RETURN
        END
    END
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Applications_CreateApplication]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM dbo.aspnet_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  dbo.aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_CheckSchemaVersion]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    dbo.aspnet_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @ApplicationName       nvarchar(256),
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Membership m, dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE dbo.aspnet_Membership
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId=@UserId
    RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @ApplicationName                        nvarchar(256),
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName AND @ApplicationId = ApplicationId
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CreateDate, @NewUserId OUTPUT
        SET @NewUserCreated = 1
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    IF ( EXISTS ( SELECT UserId
                  FROM   dbo.aspnet_Membership
                  WHERE  @NewUserId = UserId ) )
    BEGIN
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership m WITH ( UPDLOCK, HOLDLOCK )
                    WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    IF (@NewUserCreated = 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate = @CreateDate
        WHERE  @UserId = UserId
        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    INSERT INTO dbo.aspnet_Membership
                ( ApplicationId,
                  UserId,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES ( @ApplicationId,
                  @UserId,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @ApplicationName       nvarchar(256),
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.Email IS NULL
            ORDER BY m.LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY m.LoweredEmail

    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY m.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @ApplicationName       nvarchar(256),
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT u.UserId
        FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND u.LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY u.UserName


    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT u.UserId
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u
    WHERE  u.ApplicationId = @ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @ApplicationName            nvarchar(256),
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users u(NOLOCK),
            dbo.aspnet_Applications a(NOLOCK),
            dbo.aspnet_Membership m(NOLOCK)
    WHERE   u.ApplicationId = a.ApplicationId                  AND
            LastActivityDate > @DateActive                     AND
            a.LoweredApplicationName = LOWER(@ApplicationName) AND
            u.UserId = m.UserId
    RETURN(@NumOnline)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = m.Password,
            @passAns = m.PasswordAnswer,
            @PasswordFormat = m.PasswordFormat,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = u.UserId, @IsLockedOut = m.IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @CurrentTimeUtc
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTimeUtc
        WHERE   @UserId = UserId
    END


    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @ApplicationName  nvarchar(256),
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.LoweredEmail IS NULL
    ELSE
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                LOWER(@Email) = m.LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier

    IF (@UpdateLastActivity = 1)
    BEGIN
        -- select user ID from aspnet_users table
        SELECT TOP 1 @UserId = u.UserId
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId

        SELECT m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut, m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  @UserId = u.UserId AND u.UserId = m.UserId 
    END
    ELSE
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate, m.LastLoginDate, u.LastActivityDate,
            m.LastPasswordChangedDate, u.UserName, m.IsLockedOut,
            m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @ApplicationName             nvarchar(256),
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Membership WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Membership
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTimeUtc,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE dbo.aspnet_Membership
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @ApplicationName                         nvarchar(256),
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        LastLockoutDate = CONVERT( datetime, '17540101', 112 )
    WHERE @UserId = UserId

    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId, @ApplicationId = a.ApplicationId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership WITH (UPDLOCK, HOLDLOCK)
                    WHERE ApplicationId = @ApplicationId  AND @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.aspnet_Membership WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @IsApproved = m.IsApproved,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Paths_CreatePath]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Paths_CreatePath]
    @ApplicationId UNIQUEIDENTIFIER,
    @Path           NVARCHAR(256),
    @PathId         UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM dbo.aspnet_Paths WHERE LoweredPath = LOWER(@Path) AND ApplicationId = @ApplicationId))
    BEGIN
        INSERT dbo.aspnet_Paths (ApplicationId, Path, LoweredPath) VALUES (@ApplicationId, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathId FROM dbo.aspnet_Paths WHERE LOWER(@Path) = LoweredPath AND ApplicationId = @ApplicationId
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Personalization_GetApplicationId]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Personalization_GetApplicationId] (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_DeleteAllState]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_DeleteAllState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Count int OUT)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        IF (@AllUsersScope = 1)
            DELETE FROM aspnet_PersonalizationAllUsers
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)
        ELSE
            DELETE FROM aspnet_PersonalizationPerUser
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)

        SELECT @Count = @@ROWCOUNT
    END
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_FindState]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_FindState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @PageIndex              INT,
    @PageSize               INT,
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT Paths.PathId
        FROM dbo.aspnet_Paths Paths,
             ((SELECT Paths.PathId
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND AllUsers.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT DISTINCT Paths.PathId
               FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND PerUser.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path,
               SharedDataPerPath.LastUpdatedDate,
               SharedDataPerPath.SharedDataLength,
               UserDataPerPath.UserDataLength,
               UserDataPerPath.UserCount
        FROM dbo.aspnet_Paths Paths,
             ((SELECT PageIndex.ItemId AS PathId,
                      AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      DATALENGTH(AllUsers.PageSettings) AS SharedDataLength
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, #PageIndex PageIndex
               WHERE AllUsers.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT PageIndex.ItemId AS PathId,
                      SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      COUNT(*) AS UserCount
               FROM aspnet_PersonalizationPerUser PerUser, #PageIndex PageIndex
               WHERE PerUser.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
               GROUP BY PageIndex.ItemId
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC
    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT PerUser.Id
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
        WHERE Paths.ApplicationId = @ApplicationId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
              AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
        ORDER BY Paths.Path ASC, Users.UserName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path, PerUser.LastUpdatedDate, DATALENGTH(PerUser.PageSettings), Users.UserName, Users.LastActivityDate
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths, #PageIndex PageIndex
        WHERE PerUser.Id = PageIndex.ItemId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
        ORDER BY Paths.Path ASC, Users.UserName ASC
    END

    RETURN @TotalRecords
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_GetCountOfState]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_GetCountOfState] (
    @Count int OUT,
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN

    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
        IF (@AllUsersScope = 1)
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND AllUsers.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
        ELSE
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND PerUser.UserId = Users.UserId
                  AND PerUser.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
                  AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
                  AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetSharedState]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetSharedState] (
    @Count int OUT,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationAllUsers
        WHERE PathId IN
            (SELECT AllUsers.PathId
             FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
             WHERE Paths.ApplicationId = @ApplicationId
                   AND AllUsers.PathId = Paths.PathId
                   AND Paths.LoweredPath = LOWER(@Path))

        SELECT @Count = @@ROWCOUNT
    END
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetUserState]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetUserState] (
    @Count                  int                 OUT,
    @ApplicationName        NVARCHAR(256),
    @InactiveSinceDate      DATETIME            = NULL,
    @UserName               NVARCHAR(256)       = NULL,
    @Path                   NVARCHAR(256)       = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser
        WHERE Id IN (SELECT PerUser.Id
                     FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
                     WHERE Paths.ApplicationId = @ApplicationId
                           AND PerUser.UserId = Users.UserId
                           AND PerUser.PathId = Paths.PathId
                           AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
                           AND (@UserName IS NULL OR Users.LoweredUserName = LOWER(@UserName))
                           AND (@Path IS NULL OR Paths.LoweredPath = LOWER(@Path)))

        SELECT @Count = @@ROWCOUNT
    END
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationAllUsers p WHERE p.PathId = @PathId
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    DELETE FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId
    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationAllUsers SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationAllUsers(PathId, PageSettings, LastUpdatedDate) VALUES (@PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_GetPageSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationPerUser p WHERE p.PathId = @PathId AND p.UserId = @UserId
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE PathId = @PathId AND UserId = @UserId
    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_SetPageSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CurrentTimeUtc, @UserId OUTPUT
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationPerUser SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationPerUser(UserId, PathId, PageSettings, LastUpdatedDate) VALUES (@UserId, @PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteInactiveProfiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT  0
        RETURN
    END

    DELETE
    FROM    dbo.aspnet_Profile
    WHERE   UserId IN
            (   SELECT  UserId
                FROM    dbo.aspnet_Users u
                WHERE   ApplicationId = @ApplicationId
                        AND (LastActivityDate <= @InactiveSinceDate)
                        AND (
                                (@ProfileAuthOptions = 2)
                             OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                             OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                            )
            )

    SELECT  @@ROWCOUNT
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteProfiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteProfiles]
    @ApplicationName        nvarchar(256),
    @UserNames              nvarchar(4000)
AS
BEGIN
    DECLARE @UserName     nvarchar(256)
    DECLARE @CurrentPos   int
    DECLARE @NextPos      int
    DECLARE @NumDeleted   int
    DECLARE @DeletedUser  int
    DECLARE @TranStarted  bit
    DECLARE @ErrorCode    int

    SET @ErrorCode = 0
    SET @CurrentPos = 1
    SET @NumDeleted = 0
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    WHILE (@CurrentPos <= LEN(@UserNames))
    BEGIN
        SELECT @NextPos = CHARINDEX(N',', @UserNames,  @CurrentPos)
        IF (@NextPos = 0 OR @NextPos IS NULL)
            SELECT @NextPos = LEN(@UserNames) + 1

        SELECT @UserName = SUBSTRING(@UserNames, @CurrentPos, @NextPos - @CurrentPos)
        SELECT @CurrentPos = @NextPos+1

        IF (LEN(@UserName) > 0)
        BEGIN
            SELECT @DeletedUser = 0
            EXEC dbo.aspnet_Users_DeleteUser @ApplicationName, @UserName, 4, @DeletedUser OUTPUT
            IF( @@ERROR <> 0 )
            BEGIN
                SET @ErrorCode = -1
                GOTO Cleanup
            END
            IF (@DeletedUser <> 0)
                SELECT @NumDeleted = @NumDeleted + 1
        END
    END
    SELECT @NumDeleted
    IF (@TranStarted = 1)
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END
    SET @TranStarted = 0

    RETURN 0

Cleanup:
    IF (@TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END
    RETURN @ErrorCode
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT 0
        RETURN
    END

    SELECT  COUNT(*)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
    WHERE   ApplicationId = @ApplicationId
        AND u.UserId = p.UserId
        AND (LastActivityDate <= @InactiveSinceDate)
        AND (
                (@ProfileAuthOptions = 2)
                OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
            )
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProfiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @PageIndex              int,
    @PageSize               int,
    @UserNameToMatch        nvarchar(256) = NULL,
    @InactiveSinceDate      datetime      = NULL
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT  u.UserId
        FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
        WHERE   ApplicationId = @ApplicationId
            AND u.UserId = p.UserId
            AND (@InactiveSinceDate IS NULL OR LastActivityDate <= @InactiveSinceDate)
            AND (     (@ProfileAuthOptions = 2)
                   OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                   OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                 )
            AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))
        ORDER BY UserName

    SELECT  u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate,
            DATALENGTH(p.PropertyNames) + DATALENGTH(p.PropertyValuesString) + DATALENGTH(p.PropertyValuesBinary)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p, #PageIndexForUsers i
    WHERE   u.UserId = p.UserId AND p.UserId = i.UserId AND i.IndexId >= @PageLowerBound AND i.IndexId <= @PageUpperBound

    SELECT COUNT(*)
    FROM   #PageIndexForUsers

    DROP TABLE #PageIndexForUsers
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProperties]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetProperties]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN
    SELECT TOP 1 PropertyNames, PropertyValuesString, PropertyValuesBinary
    FROM         dbo.aspnet_Profile
    WHERE        UserId = @UserId

    IF (@@ROWCOUNT > 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    END
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_SetProperties]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_SetProperties]
    @ApplicationName        nvarchar(256),
    @PropertyNames          ntext,
    @PropertyValuesString   ntext,
    @PropertyValuesBinary   image,
    @UserName               nvarchar(256),
    @IsUserAnonymous        bit,
    @CurrentTimeUtc         datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
       BEGIN TRANSACTION
       SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DECLARE @UserId uniqueidentifier
    DECLARE @LastActivityDate datetime
    SELECT  @UserId = NULL
    SELECT  @LastActivityDate = @CurrentTimeUtc

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, @IsUserAnonymous, @LastActivityDate, @UserId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Users
    SET    LastActivityDate=@CurrentTimeUtc
    WHERE  UserId = @UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS( SELECT *
               FROM   dbo.aspnet_Profile
               WHERE  UserId = @UserId))
        UPDATE dbo.aspnet_Profile
        SET    PropertyNames=@PropertyNames, PropertyValuesString = @PropertyValuesString,
               PropertyValuesBinary = @PropertyValuesBinary, LastUpdatedDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    ELSE
        INSERT INTO dbo.aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate)
             VALUES (@UserId, @PropertyNames, @PropertyValuesString, @PropertyValuesBinary, @CurrentTimeUtc)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_RegisterSchemaVersion]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM dbo.aspnet_SchemaVersions WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE dbo.aspnet_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  dbo.aspnet_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RemoveAllRoleMembers]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #aspnet_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #aspnet_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #aspnet_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = 'EXEC sp_droprolemember ' + '''' + @name + ''', ''' + USER_NAME(@user_id) + ''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RestorePermissions]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(60)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #aspnet_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UnRegisterSchemaVersion]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Users_CreateUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Users_CreateUser]
    @ApplicationId    uniqueidentifier,
    @UserName         nvarchar(256),
    @IsUserAnonymous  bit,
    @LastActivityDate DATETIME,
    @UserId           uniqueidentifier OUTPUT
AS
BEGIN
    IF( @UserId IS NULL )
        SELECT @UserId = NEWID()
    ELSE
    BEGIN
        IF( EXISTS( SELECT UserId FROM dbo.aspnet_Users
                    WHERE @UserId = UserId ) )
            RETURN -1
    END

    INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
    VALUES (@ApplicationId, @UserId, @UserName, LOWER(@UserName), @IsUserAnonymous, @LastActivityDate)

    RETURN 0
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_Users_DeleteUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Users_DeleteUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId               uniqueidentifier
    SELECT  @UserId               = NULL
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   u.LoweredUserName       = LOWER(@UserName)
        AND u.ApplicationId         = a.ApplicationId
        AND LOWER(@ApplicationName) = a.LoweredApplicationName

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from Membership table if (@TablesToDeleteFrom & 1) is set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        DELETE FROM dbo.aspnet_Membership WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
               @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_UsersInRoles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_UsersInRoles WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_Profile WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM dbo.aspnet_Users WHERE @UserId = UserId)))
    BEGIN
        DELETE FROM dbo.aspnet_Users WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  SELECT @AppId, NEWID(), Name, LOWER(Name), 0, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	dbo.aspnet_Users au, @tbNames t
		  WHERE   LOWER(t.Name) = au.LoweredUserName AND au.ApplicationId = @AppId
	END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
    ORDER BY u.UserName
    RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END





GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        





GO
/****** Object:  StoredProcedure [dbo].[aspnet_WebEvent_LogEvent]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_WebEvent_LogEvent]
        @EventId         char(32),
        @EventTimeUtc    datetime,
        @EventTime       datetime,
        @EventType       nvarchar(256),
        @EventSequence   decimal(19,0),
        @EventOccurrence decimal(19,0),
        @EventCode       int,
        @EventDetailCode int,
        @Message         nvarchar(1024),
        @ApplicationPath nvarchar(256),
        @ApplicationVirtualPath nvarchar(256),
        @MachineName    nvarchar(256),
        @RequestUrl      nvarchar(1024),
        @ExceptionType   nvarchar(256),
        @Details         ntext
AS
BEGIN
    INSERT
        dbo.aspnet_WebEvent_Events
        (
            EventId,
            EventTimeUtc,
            EventTime,
            EventType,
            EventSequence,
            EventOccurrence,
            EventCode,
            EventDetailCode,
            Message,
            ApplicationPath,
            ApplicationVirtualPath,
            MachineName,
            RequestUrl,
            ExceptionType,
            Details
        )
    VALUES
    (
        @EventId,
        @EventTimeUtc,
        @EventTime,
        @EventType,
        @EventSequence,
        @EventOccurrence,
        @EventCode,
        @EventDetailCode,
        @Message,
        @ApplicationPath,
        @ApplicationVirtualPath,
        @MachineName,
        @RequestUrl,
        @ExceptionType,
        @Details
    )
END





GO
/****** Object:  UserDefinedFunction [dbo].[fSearch]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[fSearch]
(
	@word nvarchar(100),
	@limit int
)
RETURNS 
@tbl TABLE 
(
	
	ID int, 
	PageName nvarchar(max),
	Type int,
	Rank int
)
AS
BEGIN
		
insert into @tbl select distinct ID, PageName, Type, Rank from (


SELECT TOP (@limit) c.ID, Name as PageName, 0 as Type, k.Rank as Rank
FROM StoreCategories c
    INNER JOIN FREETEXTTABLE (StoreCategories, (Name), @word) AS k
    ON c.ID = k.[Key] where Rank > 1 and c.Deleted <> 1  Order by Rank desc
	union

SELECT TOP 1 p.ID as ID, Name as PageName, 1 as Type, k.Rank as Rank
FROM StoreProducts p
    INNER JOIN FREETEXTTABLE (StoreProducts, (Article, Slug), @word) AS k
    ON p.ID = k.[Key]
	where Rank > 1 and p.Deleted <> 1
ORDER BY Rank DESC
union
SELECT TOP (@limit-1) p.ID as ID, Name as PageName, 1 as Type, k.Rank as Rank
FROM StoreProducts p
    INNER JOIN FREETEXTTABLE (StoreProducts, (SearchWords, PageKeywords, Name, Article), @word) AS k
    ON p.ID = k.[Key]
	where Rank > 1 and p.Deleted <> 1
ORDER BY Rank DESC

) as sumary Order by Rank desc
	
	RETURN 
END



GO
/****** Object:  UserDefinedFunction [dbo].[getIntListByJoinedString]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[getIntListByJoinedString]
(	
	@RowData nvarchar(max),
	@SplitOn nvarchar(5)
)
RETURNS @RtnValue table(ID int)
AS
BEGIN
	Declare @Cnt int
	Set @Cnt = 1

	While (Charindex(@SplitOn,@RowData)>0)
	Begin
		Insert Into @RtnValue (ID)
		Select 
			Data = CONVERT(int, ltrim(rtrim(Substring(@RowData,1,Charindex(@SplitOn,@RowData)-1))))

		Set @RowData = Substring(@RowData,Charindex(@SplitOn,@RowData)+1,len(@RowData))
		Set @Cnt = @Cnt + 1
	End
	
	Insert Into @RtnValue (ID)
	Select Data = convert(int, ltrim(rtrim(@RowData)))
	return
END




GO
/****** Object:  UserDefinedFunction [dbo].[ToDecimal]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ToDecimal]
(
	
	@p nvarchar(max)
)
RETURNS decimal(18,5)
AS
BEGIN

declare @result decimal(18,5)
set @result = null
--return cast (replace(@p, ',', '.') as decimal(18,5))


if ISNUMERIC(replace(@p, ',', '.'))<>0
begin
	set @result = convert(decimal(18,5), replace(@p, ',', '.'))
END

RETURN @result	

END




GO
/****** Object:  Table [dbo].[AnimeBlockItems]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnimeBlockItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Link] [nvarchar](max) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[XPos] [int] NOT NULL,
	[YPos] [int] NOT NULL,
	[AnimeBlockID] [int] NOT NULL,
 CONSTRAINT [PK_AnimeBlockItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AnimeBlocks]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnimeBlocks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NULL,
	[PageID] [int] NULL,
	[Visible] [bit] NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Background] [nvarchar](500) NOT NULL,
	[Wheel] [nvarchar](500) NULL,
 CONSTRAINT [PK_AnimeBlocks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CharAnswerTemplate]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CharAnswerTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Answer] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CharAnswerTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Chat]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chat](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ChatUID] [uniqueidentifier] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[IsClosed] [bit] NOT NULL,
	[Host] [nvarchar](200) NOT NULL,
	[LastView] [datetime] NULL,
 CONSTRAINT [PK_Chat] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChatMessage]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Text] [nvarchar](500) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[ChatID] [int] NOT NULL,
	[IsRead] [bit] NOT NULL,
 CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSAuthFiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSAuthFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](400) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CMSAuthFiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageAllowedClientModuls]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageAllowedClientModuls](
	[Controller] [nvarchar](200) NOT NULL,
	[Action] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[StaticLink] [nvarchar](400) NOT NULL,
	[DinamicController] [nvarchar](200) NOT NULL,
	[DinamicAction] [nvarchar](200) NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Popup] [bit] NOT NULL,
 CONSTRAINT [PK_CMSPageAllowedClientModuls] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageCells]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageCells](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](500) NOT NULL,
	[TypeID] [int] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Hidden] [bit] NOT NULL,
	[CSS] [nvarchar](50) NULL,
 CONSTRAINT [PK_CMSPageTemplateColumn] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageCellView]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageCellView](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CellID] [int] NOT NULL,
	[Controller] [nvarchar](50) NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
	[OrderNum] [int] NOT NULL,
	[Path] [nvarchar](200) NOT NULL,
	[PageID] [int] NULL,
 CONSTRAINT [PK_CMSPageCellView] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageCellViewSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageCellViewSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[ViewID] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CMSPageCellViewSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageLangs]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageLangs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[CMSPageID] [int] NOT NULL,
	[PageName] [nvarchar](400) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Keywords] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[FullName] [nvarchar](400) NOT NULL,
	[FullNameH2] [nvarchar](400) NOT NULL,
 CONSTRAINT [PK_CMSPageLangs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageMenuCustom]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CMSPageMenuCustom](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Visible] [bit] NOT NULL,
	[UID] [nvarchar](50) NOT NULL,
	[URL] [nvarchar](500) NOT NULL,
	[OrderNum] [int] NOT NULL,
	[Image] [varbinary](max) NULL,
	[IsHeader] [bit] NOT NULL,
 CONSTRAINT [PK_CMSPageMenuCustom] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CMSPageRoleRels]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageRoleRels](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NOT NULL,
	[RoleID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_CMSPageRoleRels] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPages]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](400) NOT NULL,
	[ParentID] [int] NULL,
	[Visible] [bit] NOT NULL,
	[Type] [int] NOT NULL,
	[OrderNum] [int] NOT NULL,
	[ViewMenu] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[LastMod] [datetime] NOT NULL,
 CONSTRAINT [PK_CMSPages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageSliders]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CMSPageSliders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Visible] [bit] NOT NULL,
	[Name] [nvarchar](1000) NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[CMSPageID] [int] NULL,
	[LangID] [int] NOT NULL,
	[ViewID] [int] NULL,
	[OrderNum] [int] NOT NULL,
	[Img] [varbinary](max) NOT NULL,
	[Alt] [nvarchar](max) NOT NULL,
	[Link] [nvarchar](max) NOT NULL,
	[CategoryID] [int] NULL,
	[ProductID] [int] NULL,
 CONSTRAINT [PK_CMSPageSlider] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CMSPageTemplateCells]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageTemplateCells](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CellName] [nvarchar](200) NOT NULL,
	[CellID] [nvarchar](50) NOT NULL,
	[RowNum] [int] NOT NULL,
	[ColNum] [int] NOT NULL,
	[RowSpan] [int] NOT NULL,
	[ColSpan] [int] NOT NULL,
	[TemplateID] [int] NOT NULL,
	[MinHeight] [int] NOT NULL,
 CONSTRAINT [PK_CMSPageTemplateCells] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageTemplates]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageTemplates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateName] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Layout] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Table_1_2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageTextData]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageTextData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CMSPageID] [int] NOT NULL,
	[LangID] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[ViewID] [int] NOT NULL,
	[Visible] [bit] NOT NULL,
	[OrderNum] [int] NOT NULL,
 CONSTRAINT [PK_CMSPageTextData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageTypes]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](512) NOT NULL,
	[Ordernum] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[TemplateID] [int] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CMSPageVideo]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CMSPageVideo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FilePath] [nvarchar](400) NOT NULL,
	[PreviewPath] [nvarchar](400) NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[AutoPlay] [bit] NOT NULL,
	[CMSPageID] [int] NULL,
	[ViewID] [int] NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Visible] [bit] NOT NULL,
	[LangID] [int] NOT NULL,
	[OrderNum] [int] NOT NULL,
	[CategoryID] [int] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_CMSPageVideo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Comment] [nvarchar](500) NOT NULL,
	[Date] [datetime] NOT NULL,
	[ParentCommentID] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CommentsRatings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommentsRatings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Useful] [bit] NOT NULL,
	[CommentID] [int] NOT NULL,
 CONSTRAINT [PK_CommentsRatings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventCalendar]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventCalendar](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DayOfWeek] [int] NOT NULL,
	[PricePercent] [decimal](18, 2) NOT NULL,
	[Direction] [int] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[EventGroup] [int] NOT NULL,
	[ShowDiscount] [bit] NOT NULL,
 CONSTRAINT [PK_EventCalendar] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FilterItems]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FilterItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Help] [nvarchar](max) NULL,
	[CharID] [int] NULL,
	[FilterID] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[OrderNum] [int] NOT NULL,
	[Visible] [bit] NULL,
	[Img] [varbinary](max) NULL,
	[IsPrice] [bit] NOT NULL,
 CONSTRAINT [PK_FilterItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Filters]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Filters](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NULL,
	[PageID] [int] NULL,
	[Visible] [bit] NOT NULL,
 CONSTRAINT [PK_Filters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabelDictionary]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabelDictionary](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TextKey] [nvarchar](400) NOT NULL,
 CONSTRAINT [PK_LabelDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LabelDictionaryLangs]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabelDictionaryLangs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TranslatedLabel] [nvarchar](400) NOT NULL,
	[LabelID] [int] NOT NULL,
	[LanguageID] [int] NOT NULL,
 CONSTRAINT [PK_LabelDictionaryLangs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Languages]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Languages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShortName] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Icon] [varbinary](max) NULL,
	[Enabled] [bit] NOT NULL,
	[ByDef] [bit] NOT NULL,
	[Ordernum] [int] NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Lenta]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Lenta](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NOT NULL,
	[CellID] [int] NOT NULL,
	[TypeClass] [nvarchar](50) NOT NULL,
	[HeaderText] [nvarchar](200) NULL,
	[Link] [nvarchar](500) NULL,
	[Image] [varbinary](max) NULL,
	[Photo] [varbinary](max) NULL,
	[Author] [nvarchar](50) NULL,
	[Text] [nvarchar](max) NULL,
	[CSS1] [nvarchar](50) NULL,
	[CSS2] [nvarchar](50) NULL,
	[CSS3] [nvarchar](50) NULL,
	[CSS4] [nvarchar](50) NULL,
	[CSS5] [nvarchar](50) NULL,
	[ShowInfo] [bit] NOT NULL,
	[ShowTime] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Visible] [bit] NOT NULL,
	[FullText] [nvarchar](max) NULL,
	[ViewAmount] [int] NULL,
	[CategoryName] [nvarchar](200) NULL,
 CONSTRAINT [PK_Lenta] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LentaComments]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LentaComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CommentID] [int] NOT NULL,
	[LentaID] [int] NOT NULL,
 CONSTRAINT [PK_LentaComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MailingList]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailingList](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Header] [nvarchar](max) NOT NULL,
	[Letter] [nvarchar](max) NOT NULL,
	[LetterKey] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[IsForAdmin] [bit] NOT NULL,
	[TargetMail] [nvarchar](1000) NULL,
	[Enabled] [bit] NOT NULL,
 CONSTRAINT [PK_MailingList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MailingReplacements]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailingReplacements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Replacement] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](400) NOT NULL,
	[MailingID] [int] NOT NULL,
 CONSTRAINT [PK_MailingReplacements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapCoords]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapCoords](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[XPos] [decimal](18, 12) NOT NULL,
	[YPos] [decimal](18, 12) NOT NULL,
	[ObjectID] [int] NOT NULL,
	[OrderNum] [int] NOT NULL,
	[IsMarker] [bit] NOT NULL,
 CONSTRAINT [PK_MapCoords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapObjectComments]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapObjectComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CommentID] [int] NOT NULL,
	[ObjectID] [int] NOT NULL,
 CONSTRAINT [PK_MapObjectComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapObjectFavorites]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapObjectFavorites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[ObjectID] [int] NOT NULL,
 CONSTRAINT [PK_MapObjectFavorites] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapObjectPhotos]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MapObjectPhotos](
	[ObjectID] [int] NOT NULL,
	[RawData] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_MapObjectPhotos] PRIMARY KEY CLUSTERED 
(
	[ObjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MapObjects]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapObjects](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[TypeID] [int] NOT NULL,
	[CreatorID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ObjectType] [int] NOT NULL,
	[Address] [nvarchar](450) NULL,
	[Description] [nvarchar](450) NULL,
 CONSTRAINT [PK_MapObjects] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapObjectTypes]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapObjectTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](500) NOT NULL,
	[Icon] [nvarchar](50) NOT NULL,
	[OrderNum] [int] NOT NULL,
 CONSTRAINT [PK_ObjectTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MapSelect]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MapSelect](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Lat] [decimal](18, 12) NOT NULL,
	[Lng] [decimal](18, 12) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[Zoom] [int] NOT NULL,
	[Visible] [bit] NOT NULL,
	[OrderNum] [int] NOT NULL,
 CONSTRAINT [PK_MapSelect] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderAdresses]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderAdresses](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Town] [nvarchar](200) NULL,
	[Street] [nvarchar](200) NULL,
	[House] [nvarchar](200) NULL,
	[Building] [nvarchar](200) NULL,
	[BuildingPart] [nvarchar](200) NULL,
	[Doorway] [nvarchar](200) NULL,
	[Floor] [nvarchar](200) NULL,
	[Flat] [nvarchar](200) NULL,
	[Comment] [nvarchar](max) NULL,
	[Lat] [decimal](18, 12) NOT NULL,
	[Lng] [decimal](18, 12) NOT NULL,
	[Zoom] [int] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_OrderAdresses] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderComments]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[OrderID] [int] NOT NULL,
	[Author] [nvarchar](500) NULL,
 CONSTRAINT [PK_OrderComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDeliveryGroups]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDeliveryGroups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [nvarchar](100) NOT NULL,
	[OrderNum] [int] NOT NULL,
 CONSTRAINT [PK_OrderDeliveryGroups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDeliveryProviders]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDeliveryProviders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[UID] [nvarchar](50) NOT NULL,
	[OrderNum] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[ShowAdress] [bit] NOT NULL,
	[ShowTime] [bit] NOT NULL,
	[ShowTown] [bit] NOT NULL,
	[ShowRegions] [bit] NOT NULL,
	[ShowOrgData] [bit] NOT NULL,
	[ListType] [nvarchar](50) NULL,
	[DefaultCity] [nvarchar](50) NULL,
	[ShowIndex] [bit] NOT NULL,
	[SprinterID] [int] NOT NULL,
	[DiscountThreshold] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OrderDeliveryProviders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDeliveryRegions]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDeliveryRegions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[ImportID] [int] NULL,
	[DeliveryProviderID] [int] NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DeliveryTime] [int] NOT NULL,
	[RegionDistance] [decimal](18, 2) NULL,
	[DistanceZoneID] [int] NULL,
	[WeightZoneID] [int] NULL,
 CONSTRAINT [PK_OrderDeliveryRegions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDeliveryZoneIntervals]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDeliveryZoneIntervals](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ZoneID] [int] NOT NULL,
	[MinInterval] [decimal](18, 2) NOT NULL,
	[MaxInterval] [decimal](18, 2) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[WeightLimit] [decimal](18, 2) NULL,
	[OverWeightCost] [decimal](18, 2) NULL,
	[OverWeightUnit] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OrderDeliveryZoneIntervals] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDeliveryZones]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDeliveryZones](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[IsWeightZone] [bit] NOT NULL,
	[WeightThreshold] [decimal](18, 2) NULL,
	[AlternativeZone] [int] NULL,
 CONSTRAINT [PK_Table_1_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderID] [int] NOT NULL,
	[RegionID] [int] NULL,
	[PaymentType] [int] NOT NULL,
	[DeliveryType] [int] NOT NULL,
	[DeliveryCost] [decimal](18, 2) NOT NULL,
	[PaymentTXN] [nvarchar](50) NULL,
	[OrderCost] [decimal](18, 2) NULL,
	[DiscountCost] [decimal](18, 2) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Volume] [decimal](18, 2) NOT NULL,
	[Weight] [decimal](18, 2) NOT NULL,
	[AddressID] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderedProducts]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderedProducts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NULL,
	[SalePrice] [decimal](18, 2) NOT NULL,
	[Amount] [int] NOT NULL,
	[ImportUID] [nvarchar](50) NULL,
	[OrderID] [int] NOT NULL,
	[ProductName] [nvarchar](500) NULL,
 CONSTRAINT [PK_OrderedBooks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderPaymentDeliveryRels]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPaymentDeliveryRels](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeliveryProviderID] [int] NOT NULL,
	[PaymentProviderID] [int] NOT NULL,
 CONSTRAINT [PK_OrderPaymentDeliveryRels] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderPaymentProviders]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderPaymentProviders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[SprinterUID] [int] NOT NULL,
	[Code] [nvarchar](10) NULL,
 CONSTRAINT [PK_OrderPaymentProviders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Orders]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ImportID] [nvarchar](50) NULL,
	[StatusID] [int] NOT NULL,
	[Number] [int] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[EngName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShopCartFields]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopCartFields](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShopCartID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_ShopCartFields] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShopCartItems]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopCartItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[IsDelayed] [bit] NOT NULL,
	[ShopCartID] [int] NOT NULL,
	[IsSpec] [bit] NOT NULL,
 CONSTRAINT [PK_ShopCartItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ShopCarts]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShopCarts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[TemporaryKey] [uniqueidentifier] NULL,
	[LastRequested] [datetime] NOT NULL,
 CONSTRAINT [PK_ShopCarts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SiteCounters]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteCounters](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_SiteCounters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SiteSettings]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteSettings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Setting] [nvarchar](512) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](512) NOT NULL,
	[OrderNum] [int] NOT NULL,
	[Editor] [nvarchar](50) NOT NULL,
	[ObjectType] [nvarchar](50) NULL,
	[GroupName] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_cGlobalSettings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreCategories]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StoreCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](512) NOT NULL,
	[ParentID] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Slug] [nvarchar](128) NULL,
	[PageTitle] [nvarchar](max) NULL,
	[PageKeywords] [nvarchar](max) NULL,
	[PageDescription] [nvarchar](max) NULL,
	[OrderNum] [int] NOT NULL,
	[PageHeader] [nvarchar](max) NULL,
	[PageSubHeader] [nvarchar](max) NULL,
	[Image] [varbinary](max) NULL,
	[StaticDescription] [nvarchar](max) NULL,
	[StaticDescriptionC] [nvarchar](max) NULL,
	[StaticDescriptionB] [nvarchar](max) NULL,
	[StaticDescriptionA] [nvarchar](max) NULL,
	[StaticDescriptionLower] [nvarchar](max) NULL,
	[PageHeaderH3] [nvarchar](max) NULL,
	[PageTextH3Upper] [nvarchar](max) NULL,
	[PageTextH3Lower] [nvarchar](max) NULL,
	[ShowArticles] [bit] NOT NULL,
	[MenuUpperText] [nvarchar](max) NULL,
	[MenuImage] [nvarchar](400) NULL,
	[Deleted] [bit] NOT NULL,
	[ShowSlider] [bit] NOT NULL,
	[ShowInMenu] [bit] NOT NULL,
	[ShowInCatalog] [bit] NOT NULL,
	[ShowInBreadcrumb] [bit] NOT NULL,
	[CategoryImage] [varbinary](max) NULL,
	[CategoryImageAlt] [nvarchar](max) NULL,
	[CategoryImageTitle] [nvarchar](max) NULL,
	[TagList] [nvarchar](max) NULL,
	[MenuImageAlt] [nvarchar](max) NULL,
	[MenuImageTitle] [nvarchar](max) NULL,
	[LastMod] [datetime] NOT NULL,
	[ShowDescrAnim1] [bit] NOT NULL,
	[ShowDescrAnim3] [bit] NOT NULL,
	[ShowDescrAnim4] [bit] NOT NULL,
	[ShowDescrAnim5] [bit] NOT NULL,
	[ShowDescrAnim2] [bit] NOT NULL,
	[ShowBigIcons] [bit] NOT NULL,
 CONSTRAINT [PK_tStoreCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StoreCategoryRelations]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreCategoryRelations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RelatedProductID] [int] NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[BaseCategoryID] [int] NOT NULL,
 CONSTRAINT [PK_Table_1_3] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreCharacters]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreCharacters](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Tooltip] [nvarchar](max) NULL,
 CONSTRAINT [PK_StoreCharacters] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreCharacterToProducts]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreCharacterToProducts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[CharacterValueID] [int] NOT NULL,
 CONSTRAINT [PK_StoreCharacterToProducts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreCharacterValues]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreCharacterValues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](400) NOT NULL,
	[CharacterID] [int] NOT NULL,
	[DecimalValue] [decimal](18, 5) NULL,
 CONSTRAINT [PK_StoreCharacterValues] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreFiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreFiles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Link] [nvarchar](max) NOT NULL,
	[Download] [bit] NOT NULL,
	[OrderNum] [int] NOT NULL,
	[ProductID] [int] NULL,
	[CategoryID] [int] NULL,
 CONSTRAINT [PK_StoreFiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreImages]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](512) NULL,
	[ProductID] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[UrlPath] [nvarchar](512) NULL,
	[UrlPathThumbs] [nvarchar](512) NULL,
	[OrderNum] [int] NULL,
	[Alt] [nvarchar](512) NULL,
	[Youtube] [nvarchar](max) NULL,
 CONSTRAINT [PK_tStoreImages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreImporter]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreImporter](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ColumnName] [nvarchar](200) NOT NULL,
	[RowNum] [int] NOT NULL,
	[ColumnNum] [int] NOT NULL,
	[Header] [nvarchar](500) NOT NULL,
	[ShowInFilter] [bit] NOT NULL,
	[ShowInList] [bit] NOT NULL,
	[Priority] [int] NOT NULL,
 CONSTRAINT [PK_StoreImporter] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StorePhoto3D]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorePhoto3D](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](30) NOT NULL,
	[ProductID] [int] NOT NULL,
 CONSTRAINT [PK_StorePhoto3D] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductBlocks]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductBlocks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[OrderNum] [int] NOT NULL,
 CONSTRAINT [PK_StoreProductBlocks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductComments]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CommentID] [int] NULL,
	[ProductID] [int] NULL,
 CONSTRAINT [PK_StoreProductComments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductFavorites]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductFavorites](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[ProductID] [int] NOT NULL,
 CONSTRAINT [PK_StoreProductFavorites] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductListImage]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductListImage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_StoreProductListImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductListImageCategory]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductListImageCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CatID] [int] NOT NULL,
	[ImageID] [int] NOT NULL,
 CONSTRAINT [PK_StoreProductListImageCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductRelations]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductRelations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RelatedProductID] [int] NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[BaseProductID] [int] NOT NULL,
 CONSTRAINT [PK_StoreProductRelations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProducts]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProducts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Price] [money] NOT NULL,
	[AddDate] [datetime] NOT NULL,
	[PageTitle] [nvarchar](max) NULL,
	[PageKeywords] [nvarchar](max) NULL,
	[PageDescription] [nvarchar](max) NULL,
	[Article] [nvarchar](64) NULL,
	[Slug] [nvarchar](256) NULL,
	[OldPrice] [money] NULL,
	[IsActive] [bit] NOT NULL,
	[StaticDescription] [nvarchar](max) NULL,
	[PageH1] [nvarchar](max) NULL,
	[PageH2] [nvarchar](max) NULL,
	[PageH3] [nvarchar](max) NULL,
	[DescrptionLower] [nvarchar](max) NULL,
	[Volume] [decimal](18, 2) NULL,
	[Weight] [decimal](18, 2) NULL,
	[VoteCount] [int] NOT NULL,
	[VoteSum] [int] NOT NULL,
	[VoteOverage] [decimal](18, 2) NOT NULL,
	[Discount] [int] NOT NULL,
	[RelatedCategories] [nvarchar](max) NULL,
	[SimilarName] [nvarchar](200) NULL,
	[RecomendName] [nvarchar](200) NULL,
	[SameName] [nvarchar](200) NULL,
	[Deleted] [bit] NOT NULL,
	[SearchWords] [nvarchar](400) NOT NULL,
	[LastMod] [datetime] NOT NULL,
	[ShortName] [nvarchar](400) NOT NULL,
	[ShowDescrAnim] [bit] NOT NULL,
	[ViewCount] [int] NOT NULL,
	[PriceBaseEUR] [money] NULL,
	[PriceBaseRUR] [money] NULL,
	[BuyingRate] [decimal](18, 2) NULL,
	[ProfitRate] [decimal](18, 2) NULL,
	[DiscountRate] [decimal](18, 2) NULL,
	[SitePrice] [money] NULL,
	[DeliveryPack] [nvarchar](max) NULL,
	[ShowCompare] [bit] NOT NULL,
	[ShowDescrAnim2] [bit] NOT NULL,
	[ShowDescrAnim3] [bit] NOT NULL,
	[ShowDescrAnim4] [bit] NOT NULL,
	[ShowDescrAnim5] [bit] NOT NULL,
	[StaticDescriptionA] [nvarchar](max) NULL,
	[StaticDescriptionB] [nvarchar](max) NULL,
	[StaticDescriptionC] [nvarchar](max) NULL,
	[StaticDescriptionD] [nvarchar](max) NULL,
 CONSTRAINT [PK_tProducts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductsToCategories]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductsToCategories](
	[ProductID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[OrderNum] [int] NOT NULL,
 CONSTRAINT [PK_tShopProductsToCategories] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductTagRels]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductTagRels](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TagID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
 CONSTRAINT [PK_StoreProductTagRels] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StoreProductTags]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StoreProductTags](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_StoreProductTags] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ThemeProperties]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ThemeProperties](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[GroupName] [nvarchar](500) NOT NULL,
	[OrderNum] [int] NOT NULL,
	[ThemeID] [int] NOT NULL,
	[ValueBinary] [varbinary](max) NULL,
	[ValueText] [nvarchar](max) NULL,
	[ValuePropName] [nvarchar](200) NOT NULL,
	[ClassName] [nvarchar](200) NOT NULL,
	[Editor] [nvarchar](50) NOT NULL,
	[HelpImagePath] [nvarchar](500) NULL,
	[ValueTemplate] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ThemeProperties] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Themes]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Themes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Themes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserFavoriteLenta]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFavoriteLenta](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[LentaID] [int] NOT NULL,
 CONSTRAINT [PK_UserFavoriteLenta] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserImages]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UserImages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserMessages]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMessages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Sender] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserMessages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[UserID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Surname] [nvarchar](100) NULL,
	[Patrinomic] [nvarchar](100) NULL,
	[FromIP] [int] NULL,
	[RegDate] [datetime] NULL,
	[HomePhone] [nvarchar](50) NULL,
	[MobilePhone] [nvarchar](50) NULL,
	[Nick] [nvarchar](100) NULL,
	[Avatar] [varbinary](max) NULL,
	[Town] [nvarchar](max) NULL,
	[Region] [nvarchar](400) NULL,
	[AboutMe] [nvarchar](max) NULL,
	[Specs] [nvarchar](max) NULL,
	[IsMaster] [bit] NOT NULL,
	[ViewCount] [int] NOT NULL,
 CONSTRAINT [PK_UserProfiles_1] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserRates]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRates](
	[UserID] [uniqueidentifier] NOT NULL,
	[Rate] [int] NOT NULL,
	[IPAddress] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_UserRates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[GetSectionNames]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetSectionNames] ( @ID INT, @LID INT )
RETURNS TABLE
AS
RETURN (SELECT PageName
        FROM [dbo].CMSPageLangs
        WHERE CMSPageID = @ID and LanguageID = @LID)






GO
/****** Object:  UserDefinedFunction [dbo].[getPageList]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Антон Ковецкий
-- Create date: 14.07.2012
-- Description:	Получение структуры страниц
-- =============================================
CREATE FUNCTION [dbo].[getPageList]
(
@parentURL nvarchar(400),
@lid int
)
RETURNS TABLE 
AS

	RETURN (
	with Podmenu (ID, URL, ParentID, Visible, ViewMenu, TreeLevel, Type, FullURL, OrderNum, BreadCrumbs, LinkedBreadCrumbs, Deleted, LastMod)
	as
	(
		-- Anchor member definition
		select 
			p.ID
		,	p.URL
		,	p.ParentID
		,	p.Visible
		,	p.ViewMenu
		,	0 as TreeLevel
		,	p.Type
		,	cast(p.URL as nvarchar(max)) as FullURL
		,	p.OrderNum
		,	cast('' as nvarchar(max)) as BreadCrumbs
		,	cast('' as nvarchar(max)) as LinkedBreadCrumbs
		,	Deleted
		,	LastMod

		from 
			CMSPages p  with (NOLOCK)
--			left join CMSPageLangs pl on pl.CMSPageID = p.ID and pl.LanguageID = @lid
		where isnull(p.ParentID, 0) in 
		(
			case
				when LEN(isnull(@parentURL, '')) = 0  then 0
				else (select ID from CMSPages where URL = @parentURL)
			end
		)
		union all
		-- Recursive member definition
		select 
			p.ID
		,	p.URL
		,	p.ParentID
		,	p.Visible
		,	p.ViewMenu
		,	TreeLevel + 1 
		,	p.Type
		,	case
				when cast(FullURL as nvarchar(max)) = '' then cast(p.URL as nvarchar(400))
				else cast(cast(FullURL as nvarchar(max)) + '/' + cast(p.URL as nvarchar(400)) as nvarchar(max))
				end as FullURL
		,	p.Ordernum
		,	case
				when cast(BreadCrumbs as nvarchar(max)) = '' then cast(isnull(pl.PageName, '<noname>') as nvarchar(400))
				else cast(cast(BreadCrumbs as nvarchar(max)) + ' &mdash; ' + cast(isnull(pl.PageName, '<noname>') as nvarchar(400)) as nvarchar(max))
				end as BreadCrumbs
		,	case
				when cast(LinkedBreadCrumbs as nvarchar(max)) = '' then cast(CAST(p.ParentID as nvarchar(20))+';<a href="{0}">' + isnull(pl.PageName, '<noname>') + '</a>' as nvarchar(400))
				else cast(cast(LinkedBreadCrumbs as nvarchar(max)) + ' &mdash; ' + cast(CAST(p.ParentID as nvarchar(20))+';<a href="{0}">' + isnull(pl.PageName, '<noname>') + '</a>' as nvarchar(400)) as nvarchar(max))
				end as LinkedBreadCrumbs
		,	p.Deleted
		,	p.LastMod
		from CMSPages p with (NOLOCK)
			inner join Podmenu d on d.ID = p.ParentID
			OUTER APPLY [dbo].[GetSectionNames] ( p.ID, @lid ) pl
			)
	select 
		p.ID
		,	p.URL
		,	p.ParentID
		,	p.Visible
		,	p.ViewMenu
		,	p.TreeLevel
		,	p.Type
		,	'/' + p.FullURL as FullURL
		,	p.OrderNum
		,	p.BreadCrumbs
		,	p.LinkedBreadCrumbs
		,	isnull(pl.PageName, '<noname>') as PageName
		,	isnull(pl.FullName, '') as FullName
		,	isnull(pl.FullNameH2, '') as FullNameH2
		,	isnull(pl.Title, '') as Title
		,	isnull(pl.Keywords, '') as Keywords
		,	isnull(pl.Description, '') as Description
		,	p.Deleted
		,	p.LastMod
			

		from Podmenu p with (NOLOCK)
			left join CMSPageLangs pl on pl.CMSPageID = p.ID and pl.LanguageID = @lid

)







GO
/****** Object:  View [dbo].[vw_aspnet_Applications]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Applications]
  AS SELECT [dbo].[aspnet_Applications].[ApplicationName], [dbo].[aspnet_Applications].[LoweredApplicationName], [dbo].[aspnet_Applications].[ApplicationId], [dbo].[aspnet_Applications].[Description]
  FROM [dbo].[aspnet_Applications]
  





GO
/****** Object:  View [dbo].[vw_aspnet_MembershipUsers]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_MembershipUsers]
  AS SELECT [dbo].[aspnet_Membership].[UserId],
            [dbo].[aspnet_Membership].[PasswordFormat],
            [dbo].[aspnet_Membership].[MobilePIN],
            [dbo].[aspnet_Membership].[Email],
            [dbo].[aspnet_Membership].[LoweredEmail],
            [dbo].[aspnet_Membership].[PasswordQuestion],
            [dbo].[aspnet_Membership].[PasswordAnswer],
            [dbo].[aspnet_Membership].[IsApproved],
            [dbo].[aspnet_Membership].[IsLockedOut],
            [dbo].[aspnet_Membership].[CreateDate],
            [dbo].[aspnet_Membership].[LastLoginDate],
            [dbo].[aspnet_Membership].[LastPasswordChangedDate],
            [dbo].[aspnet_Membership].[LastLockoutDate],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptWindowStart],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptWindowStart],
            [dbo].[aspnet_Membership].[Comment],
            [dbo].[aspnet_Users].[ApplicationId],
            [dbo].[aspnet_Users].[UserName],
            [dbo].[aspnet_Users].[MobileAlias],
            [dbo].[aspnet_Users].[IsAnonymous],
            [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Membership] INNER JOIN [dbo].[aspnet_Users]
      ON [dbo].[aspnet_Membership].[UserId] = [dbo].[aspnet_Users].[UserId]
  





GO
/****** Object:  View [dbo].[vw_aspnet_Profiles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Profiles]
  AS SELECT [dbo].[aspnet_Profile].[UserId], [dbo].[aspnet_Profile].[LastUpdatedDate],
      [DataSize]=  DATALENGTH([dbo].[aspnet_Profile].[PropertyNames])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesString])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesBinary])
  FROM [dbo].[aspnet_Profile]
  





GO
/****** Object:  View [dbo].[vw_aspnet_Roles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[aspnet_Roles].[ApplicationId], [dbo].[aspnet_Roles].[RoleId], [dbo].[aspnet_Roles].[RoleName], [dbo].[aspnet_Roles].[LoweredRoleName], [dbo].[aspnet_Roles].[Description]
  FROM [dbo].[aspnet_Roles]
  





GO
/****** Object:  View [dbo].[vw_aspnet_Users]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Users]
  AS SELECT [dbo].[aspnet_Users].[ApplicationId], [dbo].[aspnet_Users].[UserId], [dbo].[aspnet_Users].[UserName], [dbo].[aspnet_Users].[LoweredUserName], [dbo].[aspnet_Users].[MobileAlias], [dbo].[aspnet_Users].[IsAnonymous], [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Users]
  





GO
/****** Object:  View [dbo].[vw_aspnet_UsersInRoles]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
  





GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_Paths]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Paths]
  AS SELECT [dbo].[aspnet_Paths].[ApplicationId], [dbo].[aspnet_Paths].[PathId], [dbo].[aspnet_Paths].[Path], [dbo].[aspnet_Paths].[LoweredPath]
  FROM [dbo].[aspnet_Paths]
  





GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_Shared]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Shared]
  AS SELECT [dbo].[aspnet_PersonalizationAllUsers].[PathId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationAllUsers].[PageSettings]), [dbo].[aspnet_PersonalizationAllUsers].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationAllUsers]
  





GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_User]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_User]
  AS SELECT [dbo].[aspnet_PersonalizationPerUser].[PathId], [dbo].[aspnet_PersonalizationPerUser].[UserId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationPerUser].[PageSettings]), [dbo].[aspnet_PersonalizationPerUser].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationPerUser]
  





GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Applications_Index]    Script Date: 12.11.2018 13:44:50 ******/
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications]
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Membership_index]    Script Date: 12.11.2018 13:44:50 ******/
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership]
(
	[ApplicationId] ASC,
	[LoweredEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Paths_index]    Script Date: 12.11.2018 13:44:50 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Paths_index] ON [dbo].[aspnet_Paths]
(
	[ApplicationId] ASC,
	[LoweredPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_PersonalizationPerUser_index1]    Script Date: 12.11.2018 13:44:50 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_PersonalizationPerUser_index1] ON [dbo].[aspnet_PersonalizationPerUser]
(
	[PathId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Roles_index1]    Script Date: 12.11.2018 13:44:50 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles]
(
	[ApplicationId] ASC,
	[LoweredRoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Users_Index]    Script Date: 12.11.2018 13:44:50 ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users]
(
	[ApplicationId] ASC,
	[LoweredUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_PersonalizationPerUser_ncindex2]    Script Date: 12.11.2018 13:44:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [aspnet_PersonalizationPerUser_ncindex2] ON [dbo].[aspnet_PersonalizationPerUser]
(
	[UserId] ASC,
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_Users_Index2]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users]
(
	[ApplicationId] ASC,
	[LastActivityDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_UsersInRoles_index]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Pages]    Script Date: 12.11.2018 13:44:50 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Pages] ON [dbo].[CMSPages]
(
	[URL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pages_ParentID]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Pages_ParentID] ON [dbo].[CMSPages]
(
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_LabelDictionary]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_LabelDictionary] ON [dbo].[LabelDictionary]
(
	[TextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Languages]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Languages] ON [dbo].[Languages]
(
	[ShortName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LentaComments]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_LentaComments] ON [dbo].[LentaComments]
(
	[LentaID] ASC,
	[CommentID] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MapCoords]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapCoords] ON [dbo].[MapCoords]
(
	[ID] ASC,
	[IsMarker] ASC,
	[ObjectID] ASC,
	[XPos] ASC,
	[YPos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MapObjectComments]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapObjectComments] ON [dbo].[MapObjectComments]
(
	[ID] ASC,
	[CommentID] ASC,
	[ObjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MapObjectFavorites]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapObjectFavorites] ON [dbo].[MapObjectFavorites]
(
	[ID] ASC,
	[ObjectID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MapObjects]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapObjects] ON [dbo].[MapObjects]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MapObjects_Addr]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapObjects_Addr] ON [dbo].[MapObjects]
(
	[Address] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MapObjects_Descr]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapObjects_Descr] ON [dbo].[MapObjects]
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MapObjects_IDS]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_MapObjects_IDS] ON [dbo].[MapObjects]
(
	[ID] ASC,
	[TypeID] ASC,
	[CreatorID] ASC,
	[ObjectType] ASC,
	[CreateDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderedBooks_DESCR]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_OrderedBooks_DESCR] ON [dbo].[OrderedProducts]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_OrderedBooks_ImportUID]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_OrderedBooks_ImportUID] ON [dbo].[OrderedProducts]
(
	[ImportUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_Created]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_Created] ON [dbo].[Orders]
(
	[CreateDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Orders_ImportID]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_ImportID] ON [dbo].[Orders]
(
	[ImportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_Number]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_Number] ON [dbo].[Orders]
(
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_Status]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_Status] ON [dbo].[Orders]
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Orders_User]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_Orders_User] ON [dbo].[Orders]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShopCartFields]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ShopCartFields] ON [dbo].[ShopCartFields]
(
	[ShopCartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ShopCartFields_Name]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ShopCartFields_Name] ON [dbo].[ShopCartFields]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShopCartItems]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ShopCartItems] ON [dbo].[ShopCartItems]
(
	[ShopCartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShopCarts_LastRequested]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ShopCarts_LastRequested] ON [dbo].[ShopCarts]
(
	[LastRequested] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShopCarts_TK]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ShopCarts_TK] ON [dbo].[ShopCarts]
(
	[TemporaryKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ShopCarts_UK]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ShopCarts_UK] ON [dbo].[ShopCarts]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductID]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_ProductID] ON [dbo].[StoreCharacterToProducts]
(
	[ProductID] ASC
)
INCLUDE ( 	[CharacterValueID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StoreCharacterToProducts]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_StoreCharacterToProducts] ON [dbo].[StoreCharacterToProducts]
(
	[CharacterValueID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_CharAndVal]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_CharAndVal] ON [dbo].[StoreCharacterValues]
(
	[CharacterID] ASC,
	[Value] ASC,
	[DecimalValue] ASC
)
INCLUDE ( 	[ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StoreImages]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_StoreImages] ON [dbo].[StoreImages]
(
	[ID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IDX_Slug]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IDX_Slug] ON [dbo].[StoreProducts]
(
	[ID] ASC
)
INCLUDE ( 	[Slug]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_StoreProducts]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_StoreProducts] ON [dbo].[StoreProducts]
(
	[Name] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StoreProductsToCategories]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_StoreProductsToCategories] ON [dbo].[StoreProductsToCategories]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_StoreProductTags]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_StoreProductTags] ON [dbo].[StoreProductTags]
(
	[Tag] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserFavoriteLenta]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_UserFavoriteLenta] ON [dbo].[UserFavoriteLenta]
(
	[LentaID] ASC,
	[UserID] ASC,
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserProfiles_UserID]    Script Date: 12.11.2018 13:44:50 ******/
CREATE NONCLUSTERED INDEX [IX_UserProfiles_UserID] ON [dbo].[UserProfiles]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  FullTextIndex     Script Date: 12.11.2018 13:44:50 ******/
CREATE FULLTEXT INDEX ON [dbo].[StoreCategories](
[Name] LANGUAGE [Russian], 
[PageHeader] LANGUAGE [Russian], 
[PageHeaderH3] LANGUAGE [Russian], 
[PageTitle] LANGUAGE [Russian])
KEY INDEX [PK_tStoreCategories]ON ([ProductSearch], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
/****** Object:  FullTextIndex     Script Date: 12.11.2018 13:44:50 ******/
CREATE FULLTEXT INDEX ON [dbo].[StoreProducts](
[Article] LANGUAGE [Russian], 
[Name] LANGUAGE [Russian], 
[PageH1] LANGUAGE [Russian], 
[PageH2] LANGUAGE [Russian], 
[PageH3] LANGUAGE [Russian], 
[PageKeywords] LANGUAGE [Russian], 
[SearchWords] LANGUAGE [Russian], 
[Slug] LANGUAGE [Russian])
KEY INDEX [PK_tProducts]ON ([ProductSearch], FILEGROUP [PRIMARY])
WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM)


GO
ALTER TABLE [dbo].[AnimeBlocks] ADD  CONSTRAINT [DF_AnimeBlocks_Visible]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD  DEFAULT (newid()) FOR [ApplicationId]
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD  DEFAULT ((0)) FOR [PasswordFormat]
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD  DEFAULT (newid()) FOR [PathId]
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[aspnet_Roles] ADD  DEFAULT (newid()) FOR [RoleId]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (NULL) FOR [MobileAlias]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT ((0)) FOR [IsAnonymous]
GO
ALTER TABLE [dbo].[Chat] ADD  CONSTRAINT [DF_Chat_StartDate]  DEFAULT (getdate()) FOR [StartDate]
GO
ALTER TABLE [dbo].[Chat] ADD  CONSTRAINT [DF_Chat_IsClosed]  DEFAULT ((0)) FOR [IsClosed]
GO
ALTER TABLE [dbo].[ChatMessage] ADD  CONSTRAINT [DF_ChatMessage_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[ChatMessage] ADD  CONSTRAINT [DF_ChatMessage_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[CMSAuthFiles] ADD  CONSTRAINT [DF_CMSAuthFiles_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[CMSPageAllowedClientModuls] ADD  CONSTRAINT [DF_CMSPageAllowedClientModuls_Description]  DEFAULT ('') FOR [Description]
GO
ALTER TABLE [dbo].[CMSPageAllowedClientModuls] ADD  CONSTRAINT [DF_CMSPageAllowedClientModuls_StaticLink]  DEFAULT ('') FOR [StaticLink]
GO
ALTER TABLE [dbo].[CMSPageAllowedClientModuls] ADD  CONSTRAINT [DF_CMSPageAllowedClientModuls_DinamicController]  DEFAULT ('') FOR [DinamicController]
GO
ALTER TABLE [dbo].[CMSPageAllowedClientModuls] ADD  CONSTRAINT [DF_CMSPageAllowedClientModuls_DinamicAction]  DEFAULT ('') FOR [DinamicAction]
GO
ALTER TABLE [dbo].[CMSPageAllowedClientModuls] ADD  CONSTRAINT [DF_CMSPageAllowedClientModuls_Popup]  DEFAULT ((0)) FOR [Popup]
GO
ALTER TABLE [dbo].[CMSPageCells] ADD  CONSTRAINT [DF_CMSPageCells_Hidden]  DEFAULT ((0)) FOR [Hidden]
GO
ALTER TABLE [dbo].[CMSPageLangs] ADD  CONSTRAINT [DF_CMSPageLangs_FullNameH2]  DEFAULT ('') FOR [FullNameH2]
GO
ALTER TABLE [dbo].[CMSPageMenuCustom] ADD  CONSTRAINT [DF_CMSPageMenuCustom_Visible]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[CMSPageMenuCustom] ADD  CONSTRAINT [DF_CMSPageMenuCustom_IsHeader]  DEFAULT ((0)) FOR [IsHeader]
GO
ALTER TABLE [dbo].[CMSPages] ADD  CONSTRAINT [DF_CMSPages_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[CMSPages] ADD  CONSTRAINT [DF_CMSPages_LastMod]  DEFAULT (getdate()) FOR [LastMod]
GO
ALTER TABLE [dbo].[CMSPageSliders] ADD  CONSTRAINT [DF_CMSPageSlider_OrderNum]  DEFAULT ((0)) FOR [OrderNum]
GO
ALTER TABLE [dbo].[CMSPageSliders] ADD  CONSTRAINT [DF_CMSPageSliders_Alt]  DEFAULT ('') FOR [Alt]
GO
ALTER TABLE [dbo].[CMSPageSliders] ADD  CONSTRAINT [DF_CMSPageSliders_Link]  DEFAULT ('') FOR [Link]
GO
ALTER TABLE [dbo].[CMSPageTemplateCells] ADD  CONSTRAINT [DF_CMSPageTemplateCells_MinHeight]  DEFAULT ((0)) FOR [MinHeight]
GO
ALTER TABLE [dbo].[CMSPageTemplates] ADD  CONSTRAINT [DF_Table_1_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CMSPageTextData] ADD  CONSTRAINT [DF_CMSPageTextData_Visible]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[CMSPageTextData] ADD  CONSTRAINT [DF_CMSPageTextData_OrderNum]  DEFAULT ((0)) FOR [OrderNum]
GO
ALTER TABLE [dbo].[CMSPageTypes] ADD  CONSTRAINT [DF_CMSPageTypes_Ordernum]  DEFAULT ((0)) FOR [Ordernum]
GO
ALTER TABLE [dbo].[CMSPageTypes] ADD  CONSTRAINT [DF_CMSPageTypes_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[CMSPageVideo] ADD  CONSTRAINT [DF_CMSPageVideo_AutoPlay]  DEFAULT ((0)) FOR [AutoPlay]
GO
ALTER TABLE [dbo].[CMSPageVideo] ADD  CONSTRAINT [DF_CMSPageVideo_Visible]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[EventCalendar] ADD  CONSTRAINT [DF_EventCalendar_EventGroup]  DEFAULT ((0)) FOR [EventGroup]
GO
ALTER TABLE [dbo].[FilterItems] ADD  CONSTRAINT [DF_FilterItems_IsPrice]  DEFAULT ((0)) FOR [IsPrice]
GO
ALTER TABLE [dbo].[Filters] ADD  CONSTRAINT [DF_Filters_Visible]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_ByDef]  DEFAULT ((0)) FOR [ByDef]
GO
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_Ordernum]  DEFAULT ((0)) FOR [Ordernum]
GO
ALTER TABLE [dbo].[Lenta] ADD  CONSTRAINT [DF_Lenta_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[MailingList] ADD  CONSTRAINT [DF_MailingList_IsForClient]  DEFAULT ((0)) FOR [IsForAdmin]
GO
ALTER TABLE [dbo].[MailingList] ADD  CONSTRAINT [DF_MailingList_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[MapCoords] ADD  CONSTRAINT [DF_MapCoords_IsMarker]  DEFAULT ((1)) FOR [IsMarker]
GO
ALTER TABLE [dbo].[MapObjectTypes] ADD  CONSTRAINT [DF_MapObjectTypes_OrderNum]  DEFAULT ((0)) FOR [OrderNum]
GO
ALTER TABLE [dbo].[MapSelect] ADD  CONSTRAINT [DF_MapSelect_OrderNum]  DEFAULT ((0)) FOR [OrderNum]
GO
ALTER TABLE [dbo].[OrderComments] ADD  CONSTRAINT [DF_OrderComments_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_GroupID]  DEFAULT ((1)) FOR [GroupID]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_ShowAdress]  DEFAULT ((1)) FOR [ShowAdress]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_ShowTime]  DEFAULT ((1)) FOR [ShowTime]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_ShowTown]  DEFAULT ((1)) FOR [ShowTown]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_ShowRegions]  DEFAULT ((1)) FOR [ShowRegions]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_ShowOrgData]  DEFAULT ((1)) FOR [ShowOrgData]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_ShowIndex]  DEFAULT ((1)) FOR [ShowIndex]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] ADD  CONSTRAINT [DF_OrderDeliveryProviders_SprinterID]  DEFAULT ((0)) FOR [SprinterID]
GO
ALTER TABLE [dbo].[OrderDeliveryRegions] ADD  CONSTRAINT [DF_OrderDeliveryRegions_Price]  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[OrderDeliveryRegions] ADD  CONSTRAINT [DF_OrderDeliveryRegions_DeliveryTime]  DEFAULT ((0)) FOR [DeliveryTime]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  CONSTRAINT [DF_OrderDetails_Volume]  DEFAULT ((0)) FOR [Volume]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  CONSTRAINT [DF_OrderDetails_Weight]  DEFAULT ((0)) FOR [Weight]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  CONSTRAINT [DF_OrderDetails_AddressID]  DEFAULT ((0)) FOR [AddressID]
GO
ALTER TABLE [dbo].[OrderedProducts] ADD  CONSTRAINT [DF_OrderedProducts_ProductName]  DEFAULT ('') FOR [ProductName]
GO
ALTER TABLE [dbo].[ShopCartItems] ADD  CONSTRAINT [DF_ShopCartItems_IsDelayed]  DEFAULT ((0)) FOR [IsDelayed]
GO
ALTER TABLE [dbo].[ShopCartItems] ADD  CONSTRAINT [DF_ShopCartItems_IsSpec]  DEFAULT ((0)) FOR [IsSpec]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_OrderNum]  DEFAULT ((0)) FOR [OrderNum]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowArticles]  DEFAULT ((1)) FOR [ShowArticles]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowSlider]  DEFAULT ((1)) FOR [ShowSlider]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowInMenu]  DEFAULT ((1)) FOR [ShowInMenu]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowInCatalog]  DEFAULT ((1)) FOR [ShowInCatalog]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowInBreadcrumb]  DEFAULT ((1)) FOR [ShowInBreadcrumb]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_LastMod]  DEFAULT (getdate()) FOR [LastMod]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowDescrAnim1]  DEFAULT ((0)) FOR [ShowDescrAnim1]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowDescrAnim23]  DEFAULT ((0)) FOR [ShowDescrAnim3]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowDescrAnim22]  DEFAULT ((0)) FOR [ShowDescrAnim4]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowDescrAnim21]  DEFAULT ((0)) FOR [ShowDescrAnim5]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowDescrAnim2]  DEFAULT ((0)) FOR [ShowDescrAnim2]
GO
ALTER TABLE [dbo].[StoreCategories] ADD  CONSTRAINT [DF_StoreCategories_ShowBigIcons]  DEFAULT ((0)) FOR [ShowBigIcons]
GO
ALTER TABLE [dbo].[StoreFiles] ADD  CONSTRAINT [DF_StoreFiles_Download]  DEFAULT ((0)) FOR [Download]
GO
ALTER TABLE [dbo].[StoreImages] ADD  CONSTRAINT [DF_tStoreImages_Enabled]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[StoreImporter] ADD  CONSTRAINT [DF_StoreImporter_ShowInFilter]  DEFAULT ((0)) FOR [ShowInFilter]
GO
ALTER TABLE [dbo].[StoreImporter] ADD  CONSTRAINT [DF_StoreImporter_ShowInList]  DEFAULT ((0)) FOR [ShowInList]
GO
ALTER TABLE [dbo].[StoreImporter] ADD  CONSTRAINT [DF_StoreImporter_Priority]  DEFAULT ((0)) FOR [Priority]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_tStoreProducts_PageID]  DEFAULT ((-1)) FOR [Price]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_tStoreProducts_AddDate]  DEFAULT (getdate()) FOR [AddDate]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_tStoreProducts_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_VoteCount]  DEFAULT ((0)) FOR [VoteCount]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_VoteSum]  DEFAULT ((0)) FOR [VoteSum]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_VoteOverage]  DEFAULT ((0)) FOR [VoteOverage]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_Discount]  DEFAULT ((0)) FOR [Discount]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_SearchWords]  DEFAULT ('') FOR [SearchWords]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_LastMod]  DEFAULT (getdate()) FOR [LastMod]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShortName]  DEFAULT ('') FOR [ShortName]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShowDescription]  DEFAULT ((0)) FOR [ShowDescrAnim]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ViewCount]  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShowCompare]  DEFAULT ((1)) FOR [ShowCompare]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShowDescrAnim4]  DEFAULT ((0)) FOR [ShowDescrAnim2]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShowDescrAnim3]  DEFAULT ((0)) FOR [ShowDescrAnim3]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShowDescrAnim2]  DEFAULT ((0)) FOR [ShowDescrAnim4]
GO
ALTER TABLE [dbo].[StoreProducts] ADD  CONSTRAINT [DF_StoreProducts_ShowDescrAnim1]  DEFAULT ((0)) FOR [ShowDescrAnim5]
GO
ALTER TABLE [dbo].[ThemeProperties] ADD  CONSTRAINT [DF_ThemeProperties_ThemeID]  DEFAULT ((1)) FOR [ThemeID]
GO
ALTER TABLE [dbo].[Themes] ADD  CONSTRAINT [DF_Themes_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserImages] ADD  CONSTRAINT [DF_UserImages_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[UserProfiles] ADD  CONSTRAINT [DF_UserProfiles_IsMaster]  DEFAULT ((0)) FOR [IsMaster]
GO
ALTER TABLE [dbo].[UserProfiles] ADD  CONSTRAINT [DF_UserProfiles_ViewCount]  DEFAULT ((0)) FOR [ViewCount]
GO
ALTER TABLE [dbo].[AnimeBlockItems]  WITH CHECK ADD  CONSTRAINT [FK_AnimeBlockItems_AnimeBlocks] FOREIGN KEY([AnimeBlockID])
REFERENCES [dbo].[AnimeBlocks] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AnimeBlockItems] CHECK CONSTRAINT [FK_AnimeBlockItems_AnimeBlocks]
GO
ALTER TABLE [dbo].[AnimeBlocks]  WITH CHECK ADD  CONSTRAINT [FK_AnimeBlocks_CMSPages] FOREIGN KEY([PageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AnimeBlocks] CHECK CONSTRAINT [FK_AnimeBlocks_CMSPages]
GO
ALTER TABLE [dbo].[AnimeBlocks]  WITH CHECK ADD  CONSTRAINT [FK_AnimeBlocks_StoreCategories] FOREIGN KEY([CatID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AnimeBlocks] CHECK CONSTRAINT [FK_AnimeBlocks_StoreCategories]
GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_aspnet_Users]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_Chat] FOREIGN KEY([ChatID])
REFERENCES [dbo].[Chat] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_Chat]
GO
ALTER TABLE [dbo].[CMSPageCells]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageCells_CMSPageTypes] FOREIGN KEY([TypeID])
REFERENCES [dbo].[CMSPageTypes] ([ID])
GO
ALTER TABLE [dbo].[CMSPageCells] CHECK CONSTRAINT [FK_CMSPageCells_CMSPageTypes]
GO
ALTER TABLE [dbo].[CMSPageCellView]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageCellView_CMSPageCells] FOREIGN KEY([CellID])
REFERENCES [dbo].[CMSPageCells] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageCellView] CHECK CONSTRAINT [FK_CMSPageCellView_CMSPageCells]
GO
ALTER TABLE [dbo].[CMSPageCellView]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageCellView_CMSPages] FOREIGN KEY([PageID])
REFERENCES [dbo].[CMSPages] ([ID])
GO
ALTER TABLE [dbo].[CMSPageCellView] CHECK CONSTRAINT [FK_CMSPageCellView_CMSPages]
GO
ALTER TABLE [dbo].[CMSPageCellViewSettings]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageCellViewSettings_CMSPageCellView] FOREIGN KEY([ViewID])
REFERENCES [dbo].[CMSPageCellView] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageCellViewSettings] CHECK CONSTRAINT [FK_CMSPageCellViewSettings_CMSPageCellView]
GO
ALTER TABLE [dbo].[CMSPageLangs]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageLangs_CMSPages] FOREIGN KEY([CMSPageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageLangs] CHECK CONSTRAINT [FK_CMSPageLangs_CMSPages]
GO
ALTER TABLE [dbo].[CMSPageLangs]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageLangs_Languages] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Languages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageLangs] CHECK CONSTRAINT [FK_CMSPageLangs_Languages]
GO
ALTER TABLE [dbo].[CMSPageRoleRels]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageRoleRels_aspnet_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageRoleRels] CHECK CONSTRAINT [FK_CMSPageRoleRels_aspnet_Roles]
GO
ALTER TABLE [dbo].[CMSPageRoleRels]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageRoleRels_CMSPages] FOREIGN KEY([PageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageRoleRels] CHECK CONSTRAINT [FK_CMSPageRoleRels_CMSPages]
GO
ALTER TABLE [dbo].[CMSPages]  WITH CHECK ADD  CONSTRAINT [FK_CMSPages_CMSPages] FOREIGN KEY([ParentID])
REFERENCES [dbo].[CMSPages] ([ID])
GO
ALTER TABLE [dbo].[CMSPages] CHECK CONSTRAINT [FK_CMSPages_CMSPages]
GO
ALTER TABLE [dbo].[CMSPages]  WITH CHECK ADD  CONSTRAINT [FK_CMSPages_CMSPageTypes] FOREIGN KEY([Type])
REFERENCES [dbo].[CMSPageTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPages] CHECK CONSTRAINT [FK_CMSPages_CMSPageTypes]
GO
ALTER TABLE [dbo].[CMSPageSliders]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageSlider_CMSPageCellView] FOREIGN KEY([ViewID])
REFERENCES [dbo].[CMSPageCellView] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageSliders] CHECK CONSTRAINT [FK_CMSPageSlider_CMSPageCellView]
GO
ALTER TABLE [dbo].[CMSPageSliders]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageSlider_CMSPages] FOREIGN KEY([CMSPageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageSliders] CHECK CONSTRAINT [FK_CMSPageSlider_CMSPages]
GO
ALTER TABLE [dbo].[CMSPageSliders]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageSlider_Languages] FOREIGN KEY([LangID])
REFERENCES [dbo].[Languages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageSliders] CHECK CONSTRAINT [FK_CMSPageSlider_Languages]
GO
ALTER TABLE [dbo].[CMSPageSliders]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageSliders_StoreCategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageSliders] CHECK CONSTRAINT [FK_CMSPageSliders_StoreCategories]
GO
ALTER TABLE [dbo].[CMSPageSliders]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageSliders_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageSliders] CHECK CONSTRAINT [FK_CMSPageSliders_StoreProducts]
GO
ALTER TABLE [dbo].[CMSPageTemplateCells]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageTemplateCells_CMSPageTemplates] FOREIGN KEY([TemplateID])
REFERENCES [dbo].[CMSPageTemplates] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageTemplateCells] CHECK CONSTRAINT [FK_CMSPageTemplateCells_CMSPageTemplates]
GO
ALTER TABLE [dbo].[CMSPageTextData]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageTextData_CMSPageCellView] FOREIGN KEY([ViewID])
REFERENCES [dbo].[CMSPageCellView] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageTextData] CHECK CONSTRAINT [FK_CMSPageTextData_CMSPageCellView]
GO
ALTER TABLE [dbo].[CMSPageTextData]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageTextData_CMSPages] FOREIGN KEY([CMSPageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageTextData] CHECK CONSTRAINT [FK_CMSPageTextData_CMSPages]
GO
ALTER TABLE [dbo].[CMSPageTextData]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageTextData_Languages] FOREIGN KEY([LangID])
REFERENCES [dbo].[Languages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageTextData] CHECK CONSTRAINT [FK_CMSPageTextData_Languages]
GO
ALTER TABLE [dbo].[CMSPageTypes]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageTypes_CMSPageTemplates] FOREIGN KEY([TemplateID])
REFERENCES [dbo].[CMSPageTemplates] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageTypes] CHECK CONSTRAINT [FK_CMSPageTypes_CMSPageTemplates]
GO
ALTER TABLE [dbo].[CMSPageVideo]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageVideo_CMSPageCellView] FOREIGN KEY([ViewID])
REFERENCES [dbo].[CMSPageCellView] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageVideo] CHECK CONSTRAINT [FK_CMSPageVideo_CMSPageCellView]
GO
ALTER TABLE [dbo].[CMSPageVideo]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageVideo_CMSPageLangs] FOREIGN KEY([LangID])
REFERENCES [dbo].[Languages] ([ID])
GO
ALTER TABLE [dbo].[CMSPageVideo] CHECK CONSTRAINT [FK_CMSPageVideo_CMSPageLangs]
GO
ALTER TABLE [dbo].[CMSPageVideo]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageVideo_CMSPages] FOREIGN KEY([CMSPageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageVideo] CHECK CONSTRAINT [FK_CMSPageVideo_CMSPages]
GO
ALTER TABLE [dbo].[CMSPageVideo]  WITH CHECK ADD  CONSTRAINT [FK_CMSPageVideo_StoreCategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CMSPageVideo] CHECK CONSTRAINT [FK_CMSPageVideo_StoreCategories]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_aspnet_Users]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Comments] FOREIGN KEY([ParentCommentID])
REFERENCES [dbo].[Comments] ([ID])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Comments]
GO
ALTER TABLE [dbo].[CommentsRatings]  WITH CHECK ADD  CONSTRAINT [FK_CommentsRatings_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[CommentsRatings] CHECK CONSTRAINT [FK_CommentsRatings_aspnet_Users]
GO
ALTER TABLE [dbo].[CommentsRatings]  WITH CHECK ADD  CONSTRAINT [FK_CommentsRatings_Comments] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comments] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CommentsRatings] CHECK CONSTRAINT [FK_CommentsRatings_Comments]
GO
ALTER TABLE [dbo].[FilterItems]  WITH CHECK ADD  CONSTRAINT [FK_FilterItems_Filters] FOREIGN KEY([FilterID])
REFERENCES [dbo].[Filters] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FilterItems] CHECK CONSTRAINT [FK_FilterItems_Filters]
GO
ALTER TABLE [dbo].[FilterItems]  WITH CHECK ADD  CONSTRAINT [FK_FilterItems_StoreChars] FOREIGN KEY([CharID])
REFERENCES [dbo].[StoreCharacters] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FilterItems] CHECK CONSTRAINT [FK_FilterItems_StoreChars]
GO
ALTER TABLE [dbo].[Filters]  WITH CHECK ADD  CONSTRAINT [FK_Filters_CMSPages] FOREIGN KEY([PageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Filters] CHECK CONSTRAINT [FK_Filters_CMSPages]
GO
ALTER TABLE [dbo].[Filters]  WITH CHECK ADD  CONSTRAINT [FK_Filters_StoreCategories] FOREIGN KEY([CatID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Filters] CHECK CONSTRAINT [FK_Filters_StoreCategories]
GO
ALTER TABLE [dbo].[LabelDictionaryLangs]  WITH CHECK ADD  CONSTRAINT [FK_LabelDictionaryLangs_LabelDictionary] FOREIGN KEY([LabelID])
REFERENCES [dbo].[LabelDictionary] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabelDictionaryLangs] CHECK CONSTRAINT [FK_LabelDictionaryLangs_LabelDictionary]
GO
ALTER TABLE [dbo].[LabelDictionaryLangs]  WITH CHECK ADD  CONSTRAINT [FK_LabelDictionaryLangs_Languages] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Languages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabelDictionaryLangs] CHECK CONSTRAINT [FK_LabelDictionaryLangs_Languages]
GO
ALTER TABLE [dbo].[Lenta]  WITH CHECK ADD  CONSTRAINT [FK_Lenta_CMSPages] FOREIGN KEY([PageID])
REFERENCES [dbo].[CMSPages] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lenta] CHECK CONSTRAINT [FK_Lenta_CMSPages]
GO
ALTER TABLE [dbo].[Lenta]  WITH CHECK ADD  CONSTRAINT [FK_Lenta_CMSPageTemplateColumn] FOREIGN KEY([CellID])
REFERENCES [dbo].[CMSPageCells] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Lenta] CHECK CONSTRAINT [FK_Lenta_CMSPageTemplateColumn]
GO
ALTER TABLE [dbo].[LentaComments]  WITH CHECK ADD  CONSTRAINT [FK_LentaComments_Comments] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comments] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LentaComments] CHECK CONSTRAINT [FK_LentaComments_Comments]
GO
ALTER TABLE [dbo].[LentaComments]  WITH CHECK ADD  CONSTRAINT [FK_LentaComments_Lenta] FOREIGN KEY([LentaID])
REFERENCES [dbo].[Lenta] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LentaComments] CHECK CONSTRAINT [FK_LentaComments_Lenta]
GO
ALTER TABLE [dbo].[MailingReplacements]  WITH CHECK ADD  CONSTRAINT [FK_MailingReplacements_MailingList] FOREIGN KEY([MailingID])
REFERENCES [dbo].[MailingList] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MailingReplacements] CHECK CONSTRAINT [FK_MailingReplacements_MailingList]
GO
ALTER TABLE [dbo].[MapCoords]  WITH CHECK ADD  CONSTRAINT [FK_MapCoords_MapObjects] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[MapObjects] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapCoords] CHECK CONSTRAINT [FK_MapCoords_MapObjects]
GO
ALTER TABLE [dbo].[MapObjectComments]  WITH CHECK ADD  CONSTRAINT [FK_MapObjectComments_Comments] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comments] ([ID])
GO
ALTER TABLE [dbo].[MapObjectComments] CHECK CONSTRAINT [FK_MapObjectComments_Comments]
GO
ALTER TABLE [dbo].[MapObjectComments]  WITH CHECK ADD  CONSTRAINT [FK_MapObjectComments_MapObjects] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[MapObjects] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapObjectComments] CHECK CONSTRAINT [FK_MapObjectComments_MapObjects]
GO
ALTER TABLE [dbo].[MapObjectFavorites]  WITH CHECK ADD  CONSTRAINT [FK_MapObjectFavorites_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[MapObjectFavorites] CHECK CONSTRAINT [FK_MapObjectFavorites_aspnet_Users]
GO
ALTER TABLE [dbo].[MapObjectFavorites]  WITH CHECK ADD  CONSTRAINT [FK_MapObjectFavorites_MapObjects] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[MapObjects] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapObjectFavorites] CHECK CONSTRAINT [FK_MapObjectFavorites_MapObjects]
GO
ALTER TABLE [dbo].[MapObjectPhotos]  WITH CHECK ADD  CONSTRAINT [FK_MapObjectPhotos_MapObjects] FOREIGN KEY([ObjectID])
REFERENCES [dbo].[MapObjects] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapObjectPhotos] CHECK CONSTRAINT [FK_MapObjectPhotos_MapObjects]
GO
ALTER TABLE [dbo].[MapObjects]  WITH CHECK ADD  CONSTRAINT [FK_MapObjects_aspnet_Users] FOREIGN KEY([CreatorID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapObjects] CHECK CONSTRAINT [FK_MapObjects_aspnet_Users]
GO
ALTER TABLE [dbo].[MapObjects]  WITH CHECK ADD  CONSTRAINT [FK_MapObjects_MapObjectTypes] FOREIGN KEY([TypeID])
REFERENCES [dbo].[MapObjectTypes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MapObjects] CHECK CONSTRAINT [FK_MapObjects_MapObjectTypes]
GO
ALTER TABLE [dbo].[OrderAdresses]  WITH CHECK ADD  CONSTRAINT [FK_OrderAdresses_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderAdresses] CHECK CONSTRAINT [FK_OrderAdresses_aspnet_Users]
GO
ALTER TABLE [dbo].[OrderComments]  WITH CHECK ADD  CONSTRAINT [FK_OrderComments_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderComments] CHECK CONSTRAINT [FK_OrderComments_Orders]
GO
ALTER TABLE [dbo].[OrderDeliveryProviders]  WITH CHECK ADD  CONSTRAINT [FK_OrderDeliveryProviders_OrderDeliveryGroups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[OrderDeliveryGroups] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDeliveryProviders] CHECK CONSTRAINT [FK_OrderDeliveryProviders_OrderDeliveryGroups]
GO
ALTER TABLE [dbo].[OrderDeliveryRegions]  WITH CHECK ADD  CONSTRAINT [FK_OrderDeliveryRegions_OrderDeliveryProviders] FOREIGN KEY([DeliveryProviderID])
REFERENCES [dbo].[OrderDeliveryProviders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDeliveryRegions] CHECK CONSTRAINT [FK_OrderDeliveryRegions_OrderDeliveryProviders]
GO
ALTER TABLE [dbo].[OrderDeliveryRegions]  WITH CHECK ADD  CONSTRAINT [FK_OrderDeliveryRegions_OrderDeliveryZones_Distance] FOREIGN KEY([DistanceZoneID])
REFERENCES [dbo].[OrderDeliveryZones] ([ID])
GO
ALTER TABLE [dbo].[OrderDeliveryRegions] CHECK CONSTRAINT [FK_OrderDeliveryRegions_OrderDeliveryZones_Distance]
GO
ALTER TABLE [dbo].[OrderDeliveryRegions]  WITH CHECK ADD  CONSTRAINT [FK_OrderDeliveryRegions_OrderDeliveryZones_Weight] FOREIGN KEY([WeightZoneID])
REFERENCES [dbo].[OrderDeliveryZones] ([ID])
GO
ALTER TABLE [dbo].[OrderDeliveryRegions] CHECK CONSTRAINT [FK_OrderDeliveryRegions_OrderDeliveryZones_Weight]
GO
ALTER TABLE [dbo].[OrderDeliveryZoneIntervals]  WITH CHECK ADD  CONSTRAINT [FK_OrderDeliveryZoneIntervals_OrderDeliveryZones] FOREIGN KEY([ZoneID])
REFERENCES [dbo].[OrderDeliveryZones] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDeliveryZoneIntervals] CHECK CONSTRAINT [FK_OrderDeliveryZoneIntervals_OrderDeliveryZones]
GO
ALTER TABLE [dbo].[OrderDeliveryZones]  WITH CHECK ADD  CONSTRAINT [FK_OrderDeliveryZones_OrderDeliveryZones] FOREIGN KEY([AlternativeZone])
REFERENCES [dbo].[OrderDeliveryZones] ([ID])
GO
ALTER TABLE [dbo].[OrderDeliveryZones] CHECK CONSTRAINT [FK_OrderDeliveryZones_OrderDeliveryZones]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_OrderDeliveryRegions] FOREIGN KEY([RegionID])
REFERENCES [dbo].[OrderDeliveryRegions] ([ID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_OrderDeliveryRegions]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders]
GO
ALTER TABLE [dbo].[OrderedProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderedProducts_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderedProducts] CHECK CONSTRAINT [FK_OrderedProducts_Orders]
GO
ALTER TABLE [dbo].[OrderedProducts]  WITH CHECK ADD  CONSTRAINT [FK_OrderedProducts_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[OrderedProducts] CHECK CONSTRAINT [FK_OrderedProducts_StoreProducts]
GO
ALTER TABLE [dbo].[OrderPaymentDeliveryRels]  WITH CHECK ADD  CONSTRAINT [FK_OrderPaymentDeliveryRels_OrderDeliveryProviders] FOREIGN KEY([DeliveryProviderID])
REFERENCES [dbo].[OrderDeliveryProviders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPaymentDeliveryRels] CHECK CONSTRAINT [FK_OrderPaymentDeliveryRels_OrderDeliveryProviders]
GO
ALTER TABLE [dbo].[OrderPaymentDeliveryRels]  WITH CHECK ADD  CONSTRAINT [FK_OrderPaymentDeliveryRels_OrderPaymentProviders] FOREIGN KEY([PaymentProviderID])
REFERENCES [dbo].[OrderPaymentProviders] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderPaymentDeliveryRels] CHECK CONSTRAINT [FK_OrderPaymentDeliveryRels_OrderPaymentProviders]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_aspnet_Users]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_OrderStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[OrderStatus] ([ID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_OrderStatus]
GO
ALTER TABLE [dbo].[ShopCartFields]  WITH CHECK ADD  CONSTRAINT [FK_ShopCartFields_ShopCarts] FOREIGN KEY([ShopCartID])
REFERENCES [dbo].[ShopCarts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopCartFields] CHECK CONSTRAINT [FK_ShopCartFields_ShopCarts]
GO
ALTER TABLE [dbo].[ShopCartItems]  WITH CHECK ADD  CONSTRAINT [FK_ShopCartItems_ShopCarts] FOREIGN KEY([ShopCartID])
REFERENCES [dbo].[ShopCarts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopCartItems] CHECK CONSTRAINT [FK_ShopCartItems_ShopCarts]
GO
ALTER TABLE [dbo].[ShopCartItems]  WITH CHECK ADD  CONSTRAINT [FK_ShopCartItems_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopCartItems] CHECK CONSTRAINT [FK_ShopCartItems_StoreProducts]
GO
ALTER TABLE [dbo].[ShopCarts]  WITH CHECK ADD  CONSTRAINT [FK_ShopCarts_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShopCarts] CHECK CONSTRAINT [FK_ShopCarts_aspnet_Users]
GO
ALTER TABLE [dbo].[StoreCategories]  WITH CHECK ADD  CONSTRAINT [FK_StoreCategories_StoreCategories] FOREIGN KEY([ParentID])
REFERENCES [dbo].[StoreCategories] ([ID])
GO
ALTER TABLE [dbo].[StoreCategories] CHECK CONSTRAINT [FK_StoreCategories_StoreCategories]
GO
ALTER TABLE [dbo].[StoreCategoryRelations]  WITH CHECK ADD  CONSTRAINT [FK_StoreCategoryRelations_StoreCategories] FOREIGN KEY([BaseCategoryID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreCategoryRelations] CHECK CONSTRAINT [FK_StoreCategoryRelations_StoreCategories]
GO
ALTER TABLE [dbo].[StoreCategoryRelations]  WITH CHECK ADD  CONSTRAINT [FK_StoreCategoryRelations_StoreProducts] FOREIGN KEY([RelatedProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreCategoryRelations] CHECK CONSTRAINT [FK_StoreCategoryRelations_StoreProducts]
GO
ALTER TABLE [dbo].[StoreCharacterToProducts]  WITH CHECK ADD  CONSTRAINT [FK_StoreCharacterToProducts_StoreCharacterValues] FOREIGN KEY([CharacterValueID])
REFERENCES [dbo].[StoreCharacterValues] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreCharacterToProducts] CHECK CONSTRAINT [FK_StoreCharacterToProducts_StoreCharacterValues]
GO
ALTER TABLE [dbo].[StoreCharacterToProducts]  WITH CHECK ADD  CONSTRAINT [FK_StoreCharacterToProducts_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreCharacterToProducts] CHECK CONSTRAINT [FK_StoreCharacterToProducts_StoreProducts]
GO
ALTER TABLE [dbo].[StoreCharacterValues]  WITH CHECK ADD  CONSTRAINT [FK_StoreCharacterValues_StoreCharacters] FOREIGN KEY([CharacterID])
REFERENCES [dbo].[StoreCharacters] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreCharacterValues] CHECK CONSTRAINT [FK_StoreCharacterValues_StoreCharacters]
GO
ALTER TABLE [dbo].[StoreFiles]  WITH CHECK ADD  CONSTRAINT [FK_StoreFiles_StoreCategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreFiles] CHECK CONSTRAINT [FK_StoreFiles_StoreCategories]
GO
ALTER TABLE [dbo].[StoreFiles]  WITH CHECK ADD  CONSTRAINT [FK_StoreFiles_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreFiles] CHECK CONSTRAINT [FK_StoreFiles_StoreProducts]
GO
ALTER TABLE [dbo].[StoreImages]  WITH CHECK ADD  CONSTRAINT [FK_tStoreImages_tStoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreImages] CHECK CONSTRAINT [FK_tStoreImages_tStoreProducts]
GO
ALTER TABLE [dbo].[StorePhoto3D]  WITH CHECK ADD  CONSTRAINT [FK_StorePhoto3D_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StorePhoto3D] CHECK CONSTRAINT [FK_StorePhoto3D_StoreProducts]
GO
ALTER TABLE [dbo].[StoreProductBlocks]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductBlocks_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductBlocks] CHECK CONSTRAINT [FK_StoreProductBlocks_StoreProducts]
GO
ALTER TABLE [dbo].[StoreProductComments]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductComments_Comments] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comments] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductComments] CHECK CONSTRAINT [FK_StoreProductComments_Comments]
GO
ALTER TABLE [dbo].[StoreProductComments]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductComments_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductComments] CHECK CONSTRAINT [FK_StoreProductComments_StoreProducts]
GO
ALTER TABLE [dbo].[StoreProductFavorites]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductFavorites_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductFavorites] CHECK CONSTRAINT [FK_StoreProductFavorites_aspnet_Users]
GO
ALTER TABLE [dbo].[StoreProductFavorites]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductFavorites_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductFavorites] CHECK CONSTRAINT [FK_StoreProductFavorites_StoreProducts]
GO
ALTER TABLE [dbo].[StoreProductListImageCategory]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductListImageCategory_StoreCategories] FOREIGN KEY([CatID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductListImageCategory] CHECK CONSTRAINT [FK_StoreProductListImageCategory_StoreCategories]
GO
ALTER TABLE [dbo].[StoreProductListImageCategory]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductListImageCategory_StoreProductListImage] FOREIGN KEY([ImageID])
REFERENCES [dbo].[StoreProductListImage] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductListImageCategory] CHECK CONSTRAINT [FK_StoreProductListImageCategory_StoreProductListImage]
GO
ALTER TABLE [dbo].[StoreProductRelations]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductRelations_StoreProducts] FOREIGN KEY([BaseProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductRelations] CHECK CONSTRAINT [FK_StoreProductRelations_StoreProducts]
GO
ALTER TABLE [dbo].[StoreProductsToCategories]  WITH CHECK ADD  CONSTRAINT [FK_tStoreProductsToCategories_tStoreCategories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[StoreCategories] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductsToCategories] CHECK CONSTRAINT [FK_tStoreProductsToCategories_tStoreCategories]
GO
ALTER TABLE [dbo].[StoreProductsToCategories]  WITH CHECK ADD  CONSTRAINT [FK_tStoreProductsToCategories_tStoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductsToCategories] CHECK CONSTRAINT [FK_tStoreProductsToCategories_tStoreProducts]
GO
ALTER TABLE [dbo].[StoreProductTagRels]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductTagRels_StoreProducts] FOREIGN KEY([ProductID])
REFERENCES [dbo].[StoreProducts] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductTagRels] CHECK CONSTRAINT [FK_StoreProductTagRels_StoreProducts]
GO
ALTER TABLE [dbo].[StoreProductTagRels]  WITH CHECK ADD  CONSTRAINT [FK_StoreProductTagRels_StoreProductTags] FOREIGN KEY([TagID])
REFERENCES [dbo].[StoreProductTags] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreProductTagRels] CHECK CONSTRAINT [FK_StoreProductTagRels_StoreProductTags]
GO
ALTER TABLE [dbo].[ThemeProperties]  WITH CHECK ADD  CONSTRAINT [FK_ThemeProperties_Themes] FOREIGN KEY([ThemeID])
REFERENCES [dbo].[Themes] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ThemeProperties] CHECK CONSTRAINT [FK_ThemeProperties_Themes]
GO
ALTER TABLE [dbo].[UserFavoriteLenta]  WITH CHECK ADD  CONSTRAINT [FK_UserFavoriteLenta_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserFavoriteLenta] CHECK CONSTRAINT [FK_UserFavoriteLenta_aspnet_Users]
GO
ALTER TABLE [dbo].[UserFavoriteLenta]  WITH CHECK ADD  CONSTRAINT [FK_UserFavoriteLenta_Lenta] FOREIGN KEY([LentaID])
REFERENCES [dbo].[Lenta] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserFavoriteLenta] CHECK CONSTRAINT [FK_UserFavoriteLenta_Lenta]
GO
ALTER TABLE [dbo].[UserImages]  WITH CHECK ADD  CONSTRAINT [FK_UserImages_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserImages] CHECK CONSTRAINT [FK_UserImages_aspnet_Users]
GO
ALTER TABLE [dbo].[UserMessages]  WITH CHECK ADD  CONSTRAINT [FK_UserMessages_aspnet_Users1] FOREIGN KEY([Sender])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[UserMessages] CHECK CONSTRAINT [FK_UserMessages_aspnet_Users1]
GO
ALTER TABLE [dbo].[UserProfiles]  WITH CHECK ADD  CONSTRAINT [FK_UserProfiles_aspnet_Users1] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProfiles] CHECK CONSTRAINT [FK_UserProfiles_aspnet_Users1]
GO
ALTER TABLE [dbo].[UserRates]  WITH CHECK ADD  CONSTRAINT [FK_UserRates_aspnet_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[aspnet_Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRates] CHECK CONSTRAINT [FK_UserRates_aspnet_Users]
GO
/****** Object:  Trigger [dbo].[CMSPageLangsModified]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[CMSPageLangsModified] 
   ON  [dbo].[CMSPageLangs]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	update CMSPages set LastMod = getdate() where ID in (select CMSPageID from INSERTED)

END

GO
/****** Object:  Trigger [dbo].[PagesModified]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[PagesModified] 
   ON  [dbo].[CMSPages]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	update CMSPages set LastMod = getdate() where ID in (select ID from INSERTED)

END

GO
/****** Object:  Trigger [dbo].[StoreCategoriesModified]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[StoreCategoriesModified] 
   ON  [dbo].[StoreCategories]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	update StoreCategories set LastMod = getdate() where ID in (select ID from INSERTED)

END

GO
/****** Object:  Trigger [dbo].[trDecimalUpdate]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anton Kovetskiy
-- Create date: 15.09.2017
-- Description:	Auto fill decimal characteristic value
-- =============================================
CREATE TRIGGER [dbo].[trDecimalUpdate] 
   ON  [dbo].[StoreCharacterValues]
   AFTER INSERT, UPDATE
AS 
BEGIN

	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	

	update t set t.DecimalValue = dbo.ToDecimal(t.Value) from StoreCharacterValues t inner join inserted i on i.ID = t.ID
	


END

GO
/****** Object:  Trigger [dbo].[StoreProductsModified]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[StoreProductsModified] 
   ON  [dbo].[StoreProducts]
   AFTER UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	update StoreProducts set LastMod = getdate() where ID in (select ID from INSERTED)

END

GO
/****** Object:  Trigger [dbo].[trPriceUpdate]    Script Date: 12.11.2018 13:44:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Anton Kovetskiy
-- Create date: 15.09.2017
-- Description:	Auto fill decimal characteristic value
-- =============================================
CREATE TRIGGER [dbo].[trPriceUpdate] 
   ON  [dbo].[StoreProducts]
   AFTER INSERT, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF EXISTS (SELECT * FROM DELETED)
	begin
		update StoreProducts set LastMod = getdate() where ID in (select ID from INSERTED)
		declare @rur money
		declare @eur money
		select @rur = i.PriceBaseRUR, @eur = i.PriceBaseEUR from inserted i
		if @rur is null and @eur is null
		begin
		update t set t.SitePrice = t.Price from StoreProducts t inner join inserted i on i.ID = t.ID
		end
	end
	else
	begin
	update t set t.SitePrice = t.Price from StoreProducts t inner join inserted i on i.ID = t.ID
	--update t set t.SitePrice = dbo.ToDecimal(t.Value) from StoreCharacterValues t inner join inserted i on i.ID = t.ID
	end


END


GO
USE [master]
GO
ALTER DATABASE [boiler-gas.ru] SET  READ_WRITE 
GO
