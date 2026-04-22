-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: dev.ttdesignco.com    Database: ttdesigndatabase_v2
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Bill`
--

DROP TABLE IF EXISTS `Bill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Bill` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `BillNo` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Date` datetime NOT NULL,
  `Type` int NOT NULL COMMENT '0 StockIn, 1 StockOut',
  `CurrencyUnit` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TotalAmount` double NOT NULL,
  `Status` int NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_bill_no_idx` (`BillNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Bill`
--

LOCK TABLES `Bill` WRITE;
/*!40000 ALTER TABLE `Bill` DISABLE KEYS */;
/*!40000 ALTER TABLE `Bill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `BillDetail`
--

DROP TABLE IF EXISTS `BillDetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `BillDetail` (
  `BillId` bigint NOT NULL,
  `ProductId` bigint NOT NULL,
  `ItemId` bigint DEFAULT NULL,
  `Unit` double NOT NULL,
  `Quantity` int NOT NULL,
  PRIMARY KEY (`ProductId`,`BillId`),
  KEY `IX_BillDetail_BillId` (`BillId`),
  CONSTRAINT `fk_bill_details` FOREIGN KEY (`BillId`) REFERENCES `Bill` (`Id`),
  CONSTRAINT `fk_bill_details_product` FOREIGN KEY (`ProductId`) REFERENCES `Product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `BillDetail`
--

LOCK TABLES `BillDetail` WRITE;
/*!40000 ALTER TABLE `BillDetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `BillDetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Calendar`
--

DROP TABLE IF EXISTS `Calendar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Calendar` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Title` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `ObjectId` bigint NOT NULL,
  `RepeatType` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'None, EveryDay, EveryWeek, EveryMonth',
  `Content` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RepeatId` bigint NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_calendar_user_idx` (`CreatedBy`,`StartTime`,`EndTime`),
  KEY `IX_Calendar_ObjectId` (`ObjectId`),
  CONSTRAINT `fk_calendar_object` FOREIGN KEY (`ObjectId`) REFERENCES `CalendarObject` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `CalendarObject`
--

DROP TABLE IF EXISTS `CalendarObject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `CalendarObject` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Type` int NOT NULL COMMENT '0 Notice, 1 Meeting Room, 2 HiEnd PC',
  `Name` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Client`
--

DROP TABLE IF EXISTS `Client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Client` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Code` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `FingerPrinter`
--

DROP TABLE IF EXISTS `FingerPrinter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `FingerPrinter` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TimesheetId` bigint NOT NULL,
  `DateIn` datetime NOT NULL,
  `DateOut` datetime NOT NULL,
  `Note` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `HourTotal` double NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_timesheet_date_idx` (`TimesheetId`,`DateIn`),
  UNIQUE KEY `IX_FingerPrinter_TimesheetId` (`TimesheetId`),
  CONSTRAINT `fk_fingerprint_timesheet` FOREIGN KEY (`TimesheetId`) REFERENCES `Timesheet` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `GoogleTimesheetKey`
--

DROP TABLE IF EXISTS `GoogleTimesheetKey`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `GoogleTimesheetKey` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `KeyName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SpreadsheetId` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ClientEmail` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrivateKey` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SheetId` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Group`
--

DROP TABLE IF EXISTS `Group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Group` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Code` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Amount` int NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `group_code_UNIQUE` (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `GroupUsers`
--

DROP TABLE IF EXISTS `GroupUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `GroupUsers` (
  `UserId` bigint NOT NULL,
  `GroupId` bigint NOT NULL,
  PRIMARY KEY (`UserId`,`GroupId`),
  UNIQUE KEY `fk_group_users_idx` (`UserId`,`GroupId`),
  KEY `fk_group_users_group` (`GroupId`),
  CONSTRAINT `fk_group_users_group` FOREIGN KEY (`GroupId`) REFERENCES `Group` (`Id`),
  CONSTRAINT `fk_group_users_user` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Holiday`
--

DROP TABLE IF EXISTS `Holiday`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Holiday` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Type` bit(1) NOT NULL DEFAULT b'0' COMMENT '0 holiday, 1 special',
  `Status` int NOT NULL COMMENT '0 pending, 1 Apply, 2 Deleting',
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_holiday_type_date_idx` (`Type`,`StartDate`,`EndDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `HolidayApply`
--

DROP TABLE IF EXISTS `HolidayApply`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `HolidayApply` (
  `HolidayId` bigint NOT NULL,
  `ApplyId` bigint NOT NULL,
  `Type` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'User, Team',
  PRIMARY KEY (`HolidayId`,`ApplyId`),
  KEY `fk_holiday_idx` (`HolidayId`),
  CONSTRAINT `fk_holiday_user` FOREIGN KEY (`HolidayId`) REFERENCES `Holiday` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Leave`
--

DROP TABLE IF EXISTS `Leave`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Leave` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `UserId` bigint NOT NULL,
  `Type` int NOT NULL COMMENT '6 Compensatory, 7 SummerVacation, 8 Annual',
  `Date` datetime NOT NULL,
  `Hours` double NOT NULL,
  `Using` double NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_leave_user_idx` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LeaveHistory`
--

DROP TABLE IF EXISTS `LeaveHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LeaveHistory` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `LeaveId` bigint DEFAULT NULL,
  `LeaveRequestId` bigint DEFAULT NULL,
  `Type` int NOT NULL COMMENT '0 AddAnnualLeave, 1 AddSummerLeave, 2 AddCompensatoryLeave, 3 TakeBackAnnualLeave, 4 TakeBackSummerLeave, 5 TakeBackCompensatoryLeave, 6 UsingAnnualLeave, 7 UsingSummerLeave, 8 UsingCompensatoryLeave',
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AnnualLeave` double NOT NULL,
  `SummerLeave` double NOT NULL,
  `CompensatoryLeave` double NOT NULL,
  `Unit` double DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_leave_history_reference_type_idx` (`LeaveId`,`LeaveRequestId`,`Type`),
  KEY `fk_leave_history_user_idx` (`CreatedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LeaveHistoryUsing`
--

DROP TABLE IF EXISTS `LeaveHistoryUsing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LeaveHistoryUsing` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `LeaveId` bigint NOT NULL,
  `LeaveRequestId` bigint NOT NULL,
  `Hours` double NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_leave_history_using_idx` (`LeaveId`,`LeaveRequestId`),
  KEY `fk_leave_history_using_leave` (`LeaveId`),
  KEY `fk_leave_history_using_leave_request` (`LeaveRequestId`),
  CONSTRAINT `fk_leave_history_using_leave_request` FOREIGN KEY (`LeaveRequestId`) REFERENCES `LeaveRequest` (`Id`),
  CONSTRAINT `FK_LeaveHistoryUsing_Leave_LeaveId` FOREIGN KEY (`LeaveId`) REFERENCES `Leave` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LeaveInformation`
--

DROP TABLE IF EXISTS `LeaveInformation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LeaveInformation` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Type` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TypeName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Detail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `LeaveDay` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartCondition` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `End` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Block` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Using` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LeaveRequest`
--

DROP TABLE IF EXISTS `LeaveRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LeaveRequest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Type` int NOT NULL COMMENT '0 SelfWedding, 1 FamilyWedding, 2 FamilyBereavement, 3 RelativeBereavement, 4 SelfMaternity, 5 FamilyMaternity, 6 Compensatory, 7 SummerVacation, 8 Annual, 9 Unpaid',
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Reason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Hour` double NOT NULL,
  `Status` int NOT NULL COMMENT '0 Pending, 1 Approve, 2 Reject',
  `Reviewer` bigint DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_leave_request_created_by_idx` (`CreatedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `LeaveRequestDetail`
--

DROP TABLE IF EXISTS `LeaveRequestDetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LeaveRequestDetail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `LeaveRequestId` bigint NOT NULL,
  `Date` datetime(6) NOT NULL,
  `Hours` double NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_leave_history_using_leave_request1` (`LeaveRequestId`),
  CONSTRAINT `fk_leave_request_detail` FOREIGN KEY (`LeaveRequestId`) REFERENCES `LeaveRequest` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Notification`
--

DROP TABLE IF EXISTS `Notification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Notification` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Content` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Type` int NOT NULL COMMENT '0 Information, 1 Approve, 2 Reject',
  `ObjectType` int NOT NULL COMMENT '0 Notification, 1 LeaveRequest, 2 OvertimeRequest, 3 WfhRequest, 4 NoticeInvite, 5 AssetInvite',
  `ObjectId` bigint NOT NULL,
  `UserName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_notification_object_idx` (`CreatedDate`,`ObjectType`,`ObjectId`),
  KEY `fk_notification_date_idx` (`CreatedDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `NotificationAssign`
--

DROP TABLE IF EXISTS `NotificationAssign`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `NotificationAssign` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `NotificationId` bigint NOT NULL,
  `Status` bit(1) NOT NULL DEFAULT b'0' COMMENT '0 unread, 1 read',
  `UserId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_notification_assign_user_idx` (`UserId`,`NotificationId`),
  KEY `IX_NotificationAssign_NotificationId` (`NotificationId`),
  CONSTRAINT `fk_notification_assign` FOREIGN KEY (`NotificationId`) REFERENCES `Notification` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OvertimeRequest`
--

DROP TABLE IF EXISTS `OvertimeRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OvertimeRequest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Reason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ProjectId` bigint NOT NULL,
  `CategoryId` bigint NOT NULL,
  `ObjectId` bigint NOT NULL,
  `Status` int NOT NULL DEFAULT '0' COMMENT '0 Pending, 1 Approve, 2 Reject, 3 Calculated',
  `Reviewer` bigint DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_leave_request_created_by_idx1` (`CreatedBy`,`StartDate`),
  KEY `fk_overtime_project` (`ProjectId`),
  CONSTRAINT `fk_overtime_project` FOREIGN KEY (`ProjectId`) REFERENCES `Project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OvertimeRequestDetail`
--

DROP TABLE IF EXISTS `OvertimeRequestDetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OvertimeRequestDetail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `OvertimeRequestId` bigint NOT NULL,
  `Type` int NOT NULL COMMENT '0 Weekday, 1 Weekend, 2 OverNight, 3 Holiday',
  `Paid` int NOT NULL COMMENT '0 Salary, 1 Compensatory',
  `Start` datetime NOT NULL,
  `End` datetime NOT NULL,
  `ActualHour` double NOT NULL,
  `Multiplier` double NOT NULL COMMENT 'hệ số tại thời điểm approve',
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_overtime_request_id_detail` (`OvertimeRequestId`),
  CONSTRAINT `fk_overtime_detail` FOREIGN KEY (`OvertimeRequestId`) REFERENCES `OvertimeRequest` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='request OT detail';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OvertimeRule`
--

DROP TABLE IF EXISTS `OvertimeRule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OvertimeRule` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Type` int NOT NULL COMMENT '0 Weekday, 1 Weekend, 2 OverNight, 3 Holiday',
  `Weekday` int DEFAULT NULL COMMENT '0 = Monday, 1 = Tuesday, 2 = Wednesday, 3 = Thursday, 4 = Friday, 5 = Saturday, 6 = Sunday',
  `HourStart` double DEFAULT NULL,
  `HourEnd` double DEFAULT NULL,
  `Multiplier` double NOT NULL COMMENT 'hệ số nhân',
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_overtime_rule_idx` (`Weekday`,`HourStart`,`HourEnd`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `OvertimeSummary`
--

DROP TABLE IF EXISTS `OvertimeSummary`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `OvertimeSummary` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Month` datetime(6) NOT NULL,
  `UserId` bigint NOT NULL,
  `HourWeekday` double NOT NULL COMMENT 'tổng hợp giờ overtime theo type, chưa nhân hệ số',
  `HourWeekend` double NOT NULL COMMENT 'tổng hợp giờ overtime theo type, chưa nhân hệ số',
  `HourOvernight` double NOT NULL COMMENT 'tổng hợp giờ overtime theo type, chưa nhân hệ số',
  `HourHoliday` double NOT NULL COMMENT 'tổng hợp giờ overtime theo type, chưa nhân hệ số',
  `HourWeekdayToCompensatory` double NOT NULL COMMENT 'tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số',
  `HourWeekendToCompensatory` double NOT NULL COMMENT 'tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số',
  `HourOvernightToCompensatory` double NOT NULL COMMENT 'tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số',
  `HourHolidayToCompensatory` double NOT NULL COMMENT 'tổng giờ overtime được quy ra nghỉ bù, chưa nhân hệ số',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_overtime_summary_idx` (`UserId`,`Month`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Product`
--

DROP TABLE IF EXISTS `Product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Product` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Species` int NOT NULL COMMENT '0 Hardware, 1 Software',
  `TypeId` bigint NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Quantity` int NOT NULL,
  `InStock` int NOT NULL,
  `Using` int NOT NULL,
  `Status` int NOT NULL COMMENT '0 Available, 1 Unavailable',
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `IX_Product_TypeId` (`TypeId`),
  CONSTRAINT `fk_product_type` FOREIGN KEY (`TypeId`) REFERENCES `ProductType` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ProductHistory`
--

DROP TABLE IF EXISTS `ProductHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProductHistory` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ItemId` bigint NOT NULL,
  `Date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Status` int NOT NULL COMMENT '0 New, 1 InStock, 2 Using, 3 OutStock',
  `Content` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_product_history_item_idx` (`ItemId`),
  CONSTRAINT `fk_product_item_history` FOREIGN KEY (`ItemId`) REFERENCES `ProductItem` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ProductItem`
--

DROP TABLE IF EXISTS `ProductItem`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProductItem` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProductId` bigint NOT NULL,
  `BillId` bigint NOT NULL,
  `Status` int NOT NULL COMMENT '0 InStock, 1 Using, 2 OutStock',
  `Serial` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `StartedDate` datetime NOT NULL,
  `ExpiryDate` datetime DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_product_item_bill_id_idx` (`BillId`),
  KEY `fk_product_item_id_idx` (`ProductId`),
  CONSTRAINT `fk_product_items` FOREIGN KEY (`ProductId`) REFERENCES `Product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ProductRequest`
--

DROP TABLE IF EXISTS `ProductRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProductRequest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Reason` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` int NOT NULL COMMENT '0 pending, 1 approverd, 2 rejected, 3 processed',
  `Reviewer` bigint NOT NULL,
  `Comment` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UserId` bigint DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `IX_ProductRequest_UserId` (`UserId`),
  CONSTRAINT `FK_ProductRequest_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ProductRequest`
--

LOCK TABLES `ProductRequest` WRITE;
/*!40000 ALTER TABLE `ProductRequest` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProductRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ProductRequestDetail`
--

DROP TABLE IF EXISTS `ProductRequestDetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProductRequestDetail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProductId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductRequestId` bigint NOT NULL,
  `ProductItemId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ProductRequestDetail_ProductItemId` (`ProductItemId`),
  KEY `IX_ProductRequestDetail_ProductRequestId` (`ProductRequestId`),
  CONSTRAINT `fk_product_request_details` FOREIGN KEY (`ProductRequestId`) REFERENCES `ProductRequest` (`Id`),
  CONSTRAINT `FK_ProductRequestDetail_ProductItem_ProductItemId` FOREIGN KEY (`ProductItemId`) REFERENCES `ProductItem` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ProductRequestDetail`
--

LOCK TABLES `ProductRequestDetail` WRITE;
/*!40000 ALTER TABLE `ProductRequestDetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProductRequestDetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ProductType`
--

DROP TABLE IF EXISTS `ProductType`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProductType` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Project`
--

DROP TABLE IF EXISTS `Project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Project` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TeamId` bigint NOT NULL,
  `ProjectManagement` bigint NOT NULL COMMENT 'PM',
  `ClientId` bigint DEFAULT NULL,
  `StartedDate` datetime NOT NULL,
  `FinishedDate` datetime DEFAULT NULL,
  `Status` int NOT NULL COMMENT '0 Pending, 1 Active, 2 End',
  `IsPublic` bit(1) DEFAULT b'0',
  `ProjectNumber` int NOT NULL,
  `QuotationHour` int NOT NULL,
  `Type` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL COMMENT 'T, I, H',
  `Code` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FiscalYear` int NOT NULL,
  `WorkingHour` double NOT NULL,
  `OvertimeHour` double NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_projects_client` (`ClientId`),
  KEY `fk_projects_leader_idx` (`ProjectManagement`),
  KEY `fk_projects_teams_idx` (`TeamId`),
  CONSTRAINT `fk_projects_client` FOREIGN KEY (`ClientId`) REFERENCES `Client` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ProjectDocument`
--

DROP TABLE IF EXISTS `ProjectDocument`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProjectDocument` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Comment` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Link` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProjectId` bigint NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_project_document_idx` (`ProjectId`),
  CONSTRAINT `fk_project_document` FOREIGN KEY (`ProjectId`) REFERENCES `Project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ProjectGroup`
--

DROP TABLE IF EXISTS `ProjectGroup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProjectGroup` (
  `ProjectId` bigint NOT NULL,
  `GroupId` bigint NOT NULL,
  PRIMARY KEY (`ProjectId`,`GroupId`),
  UNIQUE KEY `fk_project_idx` (`GroupId`,`ProjectId`),
  CONSTRAINT `fk_project_group_group` FOREIGN KEY (`GroupId`) REFERENCES `Group` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ProjectUsers`
--

DROP TABLE IF EXISTS `ProjectUsers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProjectUsers` (
  `UserId` bigint NOT NULL,
  `ProjectId` bigint NOT NULL,
  PRIMARY KEY (`UserId`,`ProjectId`),
  KEY `fk_project_user_idx` (`UserId`,`ProjectId`),
  KEY `fk_project_users_project` (`ProjectId`),
  CONSTRAINT `fk_project_users_project` FOREIGN KEY (`ProjectId`) REFERENCES `Project` (`Id`),
  CONSTRAINT `fk_project_users_user` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Role`
--

DROP TABLE IF EXISTS `Role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Role` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Type` int NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `RoleClaims`
--

DROP TABLE IF EXISTS `RoleClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `RoleClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` bigint NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_RoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_RoleClaims_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=267 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SwapDay`
--

DROP TABLE IF EXISTS `SwapDay`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SwapDay` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FromDate` datetime NOT NULL,
  `ToDate` datetime NOT NULL,
  `Content` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_swap_day_idx` (`FromDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SwapDayRefer`
--

DROP TABLE IF EXISTS `SwapDayRefer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SwapDayRefer` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `DateIn` datetime NOT NULL,
  `DateOut` datetime NOT NULL,
  `HourTotal` double NOT NULL,
  `FingerPrinterId` bigint NOT NULL,
  `SwapDayId` bigint NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_SwapDayRefer_FingerPrinterId` (`FingerPrinterId`),
  CONSTRAINT `fk_swapday_refer_timesheet` FOREIGN KEY (`FingerPrinterId`) REFERENCES `FingerPrinter` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SwapDayUser`
--

DROP TABLE IF EXISTS `SwapDayUser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SwapDayUser` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `SwapDayId` bigint NOT NULL,
  `UserId` bigint NOT NULL,
  `UserName` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_swap_day_user_idx` (`UserId`,`SwapDayId`),
  KEY `IX_SwapDayUser_SwapDayId` (`SwapDayId`),
  CONSTRAINT `fk_swap_day` FOREIGN KEY (`SwapDayId`) REFERENCES `SwapDay` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `SystemRequest`
--

DROP TABLE IF EXISTS `SystemRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SystemRequest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `UserId` bigint NOT NULL,
  `Type` int NOT NULL COMMENT '0 ActiveUser, 1 InactiveUser, 2 DefineTimesheetNextMonth, 3 DefineAnnualLeaveNextMonth, 4 DefineSummerLeave, 5 TakeBackLeave',
  `Date` datetime NOT NULL,
  `Status` int NOT NULL,
  `ObjectId` bigint NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_system_request_idx` (`Type`,`Date`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Team`
--

DROP TABLE IF EXISTS `Team`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Team` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Code` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Amount` int NOT NULL DEFAULT '0',
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `team_code_UNIQUE` (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TeamCategory`
--

DROP TABLE IF EXISTS `TeamCategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TeamCategory` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TeamId` bigint NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsUsing` tinyint(1) NOT NULL DEFAULT '0',
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_timesheet_catogeries_teams_idx` (`TeamId`,`Name`),
  CONSTRAINT `fk_timesheet_categories_team` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TeamObject`
--

DROP TABLE IF EXISTS `TeamObject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TeamObject` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TeamId` bigint NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsUsing` tinyint(1) NOT NULL DEFAULT '0',
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_timesheet_objects_teams_idx` (`TeamId`,`Name`),
  CONSTRAINT `fk_timesheet_objects_team` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TeamUser`
--

DROP TABLE IF EXISTS `TeamUser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TeamUser` (
  `TeamId` bigint NOT NULL,
  `UserId` bigint NOT NULL,
  PRIMARY KEY (`UserId`,`TeamId`),
  UNIQUE KEY `fk_team_users_idx` (`UserId`,`TeamId`),
  UNIQUE KEY `UserId` (`UserId`),
  KEY `fk_team_users_team` (`TeamId`),
  CONSTRAINT `fk_team_users_team` FOREIGN KEY (`TeamId`) REFERENCES `Team` (`Id`),
  CONSTRAINT `fk_team_users_user` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `Timesheet`
--

DROP TABLE IF EXISTS `Timesheet`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Timesheet` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `UserId` bigint NOT NULL,
  `Date` datetime NOT NULL,
  `LockBy` bigint DEFAULT NULL,
  `HolidayName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SwapDay` datetime DEFAULT NULL,
  `WfhRequestId` bigint DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_timesheet_user_date_idx` (`UserId`,`Date`),
  KEY `fk_timesheet_user_idx` (`UserId`),
  KEY `IX_Timesheet_WfhRequestId` (`WfhRequestId`),
  CONSTRAINT `fk_timesheet_user` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`),
  CONSTRAINT `fk_timesheet_wfhRequest` FOREIGN KEY (`WfhRequestId`) REFERENCES `WfhRequest` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TimesheetDetail`
--

DROP TABLE IF EXISTS `TimesheetDetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TimesheetDetail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Type` int NOT NULL COMMENT '0 Project, 1 Overtime, 2 UnpaidLeave, 3 PaidLeave',
  `TimesheetId` bigint NOT NULL,
  `ProjectId` bigint NOT NULL,
  `ReferenceId` bigint NOT NULL COMMENT 'reference to overtime_request, leave_request, holiday_id',
  `TimesheetCategoryId` bigint DEFAULT NULL,
  `TimesheetObjectId` bigint DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Hours` double NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_timesheet_detail_project_id_idx` (`ProjectId`),
  KEY `fk_timesheet_detail_timesheet_id_idx` (`TimesheetId`),
  KEY `fk_timesheet_detail_type_idx` (`ReferenceId`,`Type`),
  CONSTRAINT `fk_timesheet_detail_timesheet` FOREIGN KEY (`TimesheetId`) REFERENCES `Timesheet` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `TimesheetReport`
--

DROP TABLE IF EXISTS `TimesheetReport`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TimesheetReport` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TimesheetId` bigint NOT NULL,
  `ProjectId` bigint NOT NULL,
  `Hours` double NOT NULL,
  `OvertimeHours` double NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `fk_report_timesheet_id_idx` (`TimesheetId`,`ProjectId`),
  CONSTRAINT `fk_timesheet_report` FOREIGN KEY (`TimesheetId`) REFERENCES `Timesheet` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `User`
--

DROP TABLE IF EXISTS `User`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `User` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `FullName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DateStartWork` datetime DEFAULT NULL,
  `IsActive` bit(1) NOT NULL DEFAULT b'1' COMMENT '0 Inactive, 1 Active',
  `State` int NOT NULL DEFAULT '0' COMMENT '0 Available, 1 WFH, 2 Business, 3 Busy, 4 Unavailable',
  `StaffId` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Position` int NOT NULL COMMENT '0 System, 1 GM, 2 Leader, 3 SubLeader, 4 Official, 5 Probationary, 6 Intership',
  `Avatar` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint DEFAULT NULL,
  `ModifiedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=101 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UserClaims`
--

DROP TABLE IF EXISTS `UserClaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserClaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` bigint NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_UserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_UserClaims_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UserInfo`
--

DROP TABLE IF EXISTS `UserInfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserInfo` (
  `UserId` bigint NOT NULL AUTO_INCREMENT,
  `PhoneNumber` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Gender` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'male',
  `Birthday` datetime DEFAULT NULL,
  `IdNo` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IssuedTo` datetime DEFAULT NULL,
  `IssuedBy` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SocialInsuranceBookNo` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `AboutMe` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `FingerId` int NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `user_id_UNIQUE` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UserLogins`
--

DROP TABLE IF EXISTS `UserLogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserLogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` bigint NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_UserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_UserLogins_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UserRoles`
--

DROP TABLE IF EXISTS `UserRoles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserRoles` (
  `UserId` bigint NOT NULL,
  `RoleId` bigint NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_UserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_UserRoles_Role_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Role` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_UserRoles_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UserSetting`
--

DROP TABLE IF EXISTS `UserSetting`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserSetting` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `EmailNotification` bit(1) NOT NULL DEFAULT b'1',
  `Timezone` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'BangKok, Ha Noi, Jakarta (UTC +7)',
  `Language` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'English',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `user_id_UNIQUE1` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `UserTokens`
--

DROP TABLE IF EXISTS `UserTokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserTokens` (
  `UserId` bigint NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_UserTokens_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `WfhRequest`
--

DROP TABLE IF EXISTS `WfhRequest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `WfhRequest` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `StartTime` datetime NOT NULL,
  `EndTime` datetime NOT NULL,
  `Reason` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` int NOT NULL DEFAULT '0' COMMENT '0 Pending, 1 Approve, 2 Reject',
  `Reviewer` bigint DEFAULT NULL,
  `CreatedBy` bigint NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifiedBy` bigint NOT NULL,
  `ModifiedDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_leave_request_created_by_idx2` (`CreatedBy`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-06-25 14:17:26
