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
-- Dumping data for table `Calendar`
--

LOCK TABLES `Calendar` WRITE;
/*!40000 ALTER TABLE `Calendar` DISABLE KEYS */;
/*!40000 ALTER TABLE `Calendar` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `CalendarObject`
--

LOCK TABLES `CalendarObject` WRITE;
/*!40000 ALTER TABLE `CalendarObject` DISABLE KEYS */;
/*!40000 ALTER TABLE `CalendarObject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Client`
--

LOCK TABLES `Client` WRITE;
/*!40000 ALTER TABLE `Client` DISABLE KEYS */;
/*!40000 ALTER TABLE `Client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `FingerPrinter`
--

LOCK TABLES `FingerPrinter` WRITE;
/*!40000 ALTER TABLE `FingerPrinter` DISABLE KEYS */;
/*!40000 ALTER TABLE `FingerPrinter` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `GoogleTimesheetKey`
--

LOCK TABLES `GoogleTimesheetKey` WRITE;
/*!40000 ALTER TABLE `GoogleTimesheetKey` DISABLE KEYS */;
/*!40000 ALTER TABLE `GoogleTimesheetKey` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Group`
--

LOCK TABLES `Group` WRITE;
/*!40000 ALTER TABLE `Group` DISABLE KEYS */;
/*!40000 ALTER TABLE `Group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `GroupUsers`
--

LOCK TABLES `GroupUsers` WRITE;
/*!40000 ALTER TABLE `GroupUsers` DISABLE KEYS */;
/*!40000 ALTER TABLE `GroupUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Holiday`
--

LOCK TABLES `Holiday` WRITE;
/*!40000 ALTER TABLE `Holiday` DISABLE KEYS */;
/*!40000 ALTER TABLE `Holiday` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `HolidayApply`
--

LOCK TABLES `HolidayApply` WRITE;
/*!40000 ALTER TABLE `HolidayApply` DISABLE KEYS */;
/*!40000 ALTER TABLE `HolidayApply` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Leave`
--

LOCK TABLES `Leave` WRITE;
/*!40000 ALTER TABLE `Leave` DISABLE KEYS */;
/*!40000 ALTER TABLE `Leave` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `LeaveHistory`
--

LOCK TABLES `LeaveHistory` WRITE;
/*!40000 ALTER TABLE `LeaveHistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `LeaveHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `LeaveHistoryUsing`
--

LOCK TABLES `LeaveHistoryUsing` WRITE;
/*!40000 ALTER TABLE `LeaveHistoryUsing` DISABLE KEYS */;
/*!40000 ALTER TABLE `LeaveHistoryUsing` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `LeaveInformation`
--

LOCK TABLES `LeaveInformation` WRITE;
/*!40000 ALTER TABLE `LeaveInformation` DISABLE KEYS */;
/*!40000 ALTER TABLE `LeaveInformation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `LeaveRequest`
--

LOCK TABLES `LeaveRequest` WRITE;
/*!40000 ALTER TABLE `LeaveRequest` DISABLE KEYS */;
/*!40000 ALTER TABLE `LeaveRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `LeaveRequestDetail`
--

LOCK TABLES `LeaveRequestDetail` WRITE;
/*!40000 ALTER TABLE `LeaveRequestDetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `LeaveRequestDetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Notification`
--

LOCK TABLES `Notification` WRITE;
/*!40000 ALTER TABLE `Notification` DISABLE KEYS */;
/*!40000 ALTER TABLE `Notification` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `NotificationAssign`
--

LOCK TABLES `NotificationAssign` WRITE;
/*!40000 ALTER TABLE `NotificationAssign` DISABLE KEYS */;
/*!40000 ALTER TABLE `NotificationAssign` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `OvertimeRequest`
--

LOCK TABLES `OvertimeRequest` WRITE;
/*!40000 ALTER TABLE `OvertimeRequest` DISABLE KEYS */;
/*!40000 ALTER TABLE `OvertimeRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `OvertimeRequestDetail`
--

LOCK TABLES `OvertimeRequestDetail` WRITE;
/*!40000 ALTER TABLE `OvertimeRequestDetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `OvertimeRequestDetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `OvertimeRule`
--

LOCK TABLES `OvertimeRule` WRITE;
/*!40000 ALTER TABLE `OvertimeRule` DISABLE KEYS */;
/*!40000 ALTER TABLE `OvertimeRule` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `OvertimeSummary`
--

LOCK TABLES `OvertimeSummary` WRITE;
/*!40000 ALTER TABLE `OvertimeSummary` DISABLE KEYS */;
/*!40000 ALTER TABLE `OvertimeSummary` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Product`
--

LOCK TABLES `Product` WRITE;
/*!40000 ALTER TABLE `Product` DISABLE KEYS */;
/*!40000 ALTER TABLE `Product` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProductHistory`
--

LOCK TABLES `ProductHistory` WRITE;
/*!40000 ALTER TABLE `ProductHistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProductHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProductItem`
--

LOCK TABLES `ProductItem` WRITE;
/*!40000 ALTER TABLE `ProductItem` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProductItem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProductRequest`
--

LOCK TABLES `ProductRequest` WRITE;
/*!40000 ALTER TABLE `ProductRequest` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProductRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProductType`
--

LOCK TABLES `ProductType` WRITE;
/*!40000 ALTER TABLE `ProductType` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProductType` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Project`
--

LOCK TABLES `Project` WRITE;
/*!40000 ALTER TABLE `Project` DISABLE KEYS */;
/*!40000 ALTER TABLE `Project` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProjectDocument`
--

LOCK TABLES `ProjectDocument` WRITE;
/*!40000 ALTER TABLE `ProjectDocument` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProjectDocument` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProjectGroup`
--

LOCK TABLES `ProjectGroup` WRITE;
/*!40000 ALTER TABLE `ProjectGroup` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProjectGroup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `ProjectUsers`
--

LOCK TABLES `ProjectUsers` WRITE;
/*!40000 ALTER TABLE `ProjectUsers` DISABLE KEYS */;
/*!40000 ALTER TABLE `ProjectUsers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Role`
--

LOCK TABLES `Role` WRITE;
/*!40000 ALTER TABLE `Role` DISABLE KEYS */;
INSERT INTO `Role` VALUES (1,0,1,'2024-06-25 07:07:55',1,'2024-06-25 07:07:55','System','SYSTEM','7d19052e-17ed-4854-b167-2f693fdfbacf'),(2,0,1,'2024-06-25 07:09:08',1,'2024-06-25 07:09:08','GM','GM','53724bc5-8027-40f4-a1cd-d06d6c2555b8'),(3,0,1,'2024-06-25 07:10:22',1,'2024-06-25 07:10:22','Team Lead','TEAM LEAD','dd5d9872-23a0-4301-9cca-5f9f0776ae08'),(4,0,1,'2024-06-25 07:11:10',1,'2024-06-25 07:11:10','Sub Lead','SUB LEAD','a34b93f4-8764-481b-a054-a281a7817818'),(5,0,1,'2024-06-25 07:11:57',1,'2024-06-25 07:11:57','PM','PM','aac1e2b7-b66f-4436-82ff-61fe864a92e9'),(6,0,1,'2024-06-25 07:12:16',1,'2024-06-25 07:12:16','Official Staff','OFFICIAL STAFF','79376657-7732-4449-a777-2c2708860edf'),(7,0,1,'2024-06-25 07:12:24',1,'2024-06-25 07:12:24','Probationary Staff','PROBATIONARY STAFF','7c00c586-26a4-4766-8a28-cef4e8f77e93'),(8,0,1,'2024-06-25 07:12:33',1,'2024-06-25 07:12:33','Internship','INTERNSHIP','1b8ef2d7-1845-49ff-95f1-82df22c751f6');
/*!40000 ALTER TABLE `Role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `RoleClaims`
--

LOCK TABLES `RoleClaims` WRITE;
/*!40000 ALTER TABLE `RoleClaims` DISABLE KEYS */;
INSERT INTO `RoleClaims` VALUES (1,1,'staff:dashboard','view'),(2,1,'staff:timesheet','view'),(3,1,'staff:timesheet:other','view'),(4,1,'staff:timesheet:other','lock'),(5,1,'staff:overtime','view'),(6,1,'staff:leave','view'),(7,1,'staff:calendar','view'),(8,1,'staff:asset','view'),(9,1,'staff:wfh','view'),(10,1,'admin:user','view'),(11,1,'admin:user','create'),(12,1,'admin:user','edit'),(13,1,'admin:user','inactive'),(14,1,'admin:user','delete'),(15,1,'admin:team','view'),(16,1,'admin:team','create'),(17,1,'admin:team','edit'),(18,1,'admin:team','delete'),(19,1,'admin:group','view'),(20,1,'admin:group','create'),(21,1,'admin:group','edit'),(22,1,'admin:group','delete'),(23,1,'admin:role','view'),(24,1,'admin:role','create'),(25,1,'admin:role','edit'),(26,1,'admin:role','delete'),(27,1,'admin:project','view'),(28,1,'admin:project','create'),(29,1,'admin:project','edit'),(30,1,'admin:project','delete'),(31,1,'admin:analysis','view'),(32,1,'admin:analysis','report'),(33,1,'admin:object','view'),(34,1,'admin:object','create'),(35,1,'admin:object','edit'),(36,1,'admin:object','delete'),(37,1,'admin:category','view'),(38,1,'admin:category','create'),(39,1,'admin:category','edit'),(40,1,'admin:category','delete'),(41,1,'admin:fingerprint','view'),(42,1,'admin:fingerprint','edit'),(43,1,'admin:fingerprint','delete'),(44,1,'admin:swapday','view'),(45,1,'admin:swapday','create'),(46,1,'admin:swapday','edit'),(47,1,'admin:swapday','delete'),(48,1,'admin:overtime','view'),(49,1,'admin:overtime','approve'),(50,1,'admin:overtime','report'),(51,1,'admin:overtime','setting'),(52,1,'admin:leaveform','view'),(53,1,'admin:leaveform','approve'),(54,1,'admin:leaveform','report'),(55,1,'admin:leaveform','holiday'),(56,1,'admin:asset','view'),(57,1,'admin:asset','approve'),(58,1,'admin:asset','report'),(59,1,'admin:asset','assign'),(60,1,'admin:product','view'),(61,1,'admin:product','create'),(62,1,'admin:product','edit'),(63,1,'admin:product','delete'),(64,1,'admin:bill','view'),(65,1,'admin:bill','create'),(66,1,'admin:bill','edit'),(67,1,'admin:bill','delete'),(68,1,'admin:wfh','view'),(69,1,'admin:wfh','approve'),(70,2,'staff:dashboard','view'),(71,2,'staff:timesheet','view'),(72,2,'staff:timesheet:other','view'),(73,2,'staff:timesheet:other','lock'),(74,2,'staff:overtime','view'),(75,2,'staff:leave','view'),(76,2,'staff:calendar','view'),(77,2,'staff:asset','view'),(78,2,'staff:wfh','view'),(79,2,'admin:user','view'),(80,2,'admin:user','create'),(81,2,'admin:user','edit'),(82,2,'admin:user','inactive'),(83,2,'admin:user','delete'),(84,2,'admin:team','view'),(85,2,'admin:team','create'),(86,2,'admin:team','edit'),(87,2,'admin:team','delete'),(88,2,'admin:group','view'),(89,2,'admin:group','create'),(90,2,'admin:group','edit'),(91,2,'admin:group','delete'),(92,2,'admin:role','view'),(93,2,'admin:role','create'),(94,2,'admin:role','edit'),(95,2,'admin:role','delete'),(96,2,'admin:project','view'),(97,2,'admin:project','create'),(98,2,'admin:project','edit'),(99,2,'admin:project','delete'),(100,2,'admin:analysis','view'),(101,2,'admin:analysis','report'),(102,2,'admin:object','view'),(103,2,'admin:object','create'),(104,2,'admin:object','edit'),(105,2,'admin:object','delete'),(106,2,'admin:category','view'),(107,2,'admin:category','create'),(108,2,'admin:category','edit'),(109,2,'admin:category','delete'),(110,2,'admin:fingerprint','view'),(111,2,'admin:fingerprint','edit'),(112,2,'admin:fingerprint','delete'),(113,2,'admin:swapday','view'),(114,2,'admin:swapday','create'),(115,2,'admin:swapday','edit'),(116,2,'admin:swapday','delete'),(117,2,'admin:overtime','view'),(118,2,'admin:overtime','approve'),(119,2,'admin:overtime','report'),(120,2,'admin:overtime','setting'),(121,2,'admin:leaveform','view'),(122,2,'admin:leaveform','approve'),(123,2,'admin:leaveform','report'),(124,2,'admin:leaveform','holiday'),(125,2,'admin:asset','view'),(126,2,'admin:asset','approve'),(127,2,'admin:asset','report'),(128,2,'admin:asset','assign'),(129,2,'admin:product','view'),(130,2,'admin:product','create'),(131,2,'admin:product','edit'),(132,2,'admin:product','delete'),(133,2,'admin:bill','view'),(134,2,'admin:bill','create'),(135,2,'admin:bill','edit'),(136,2,'admin:bill','delete'),(137,2,'admin:wfh','view'),(138,2,'admin:wfh','approve'),(139,3,'staff:dashboard','view'),(140,3,'staff:timesheet','view'),(141,3,'staff:timesheet:other','view'),(142,3,'staff:timesheet:other','lock'),(143,3,'staff:overtime','view'),(144,3,'staff:leave','view'),(145,3,'staff:asset','view'),(146,3,'staff:calendar','view'),(147,3,'staff:wfh','view'),(148,3,'admin:user','view'),(149,3,'admin:user','create'),(150,3,'admin:user','edit'),(151,3,'admin:user','inactive'),(152,3,'admin:group','view'),(153,3,'admin:group','create'),(154,3,'admin:group','edit'),(155,3,'admin:group','delete'),(156,3,'admin:role','view'),(157,3,'admin:project','view'),(158,3,'admin:project','create'),(159,3,'admin:project','edit'),(160,3,'admin:project','delete'),(161,3,'admin:analysis','view'),(162,3,'admin:analysis','report'),(163,3,'admin:object','view'),(164,3,'admin:object','create'),(165,3,'admin:object','edit'),(166,3,'admin:object','delete'),(167,3,'admin:category','view'),(168,3,'admin:category','create'),(169,3,'admin:category','edit'),(170,3,'admin:category','delete'),(171,3,'admin:fingerprint','view'),(172,3,'admin:fingerprint','edit'),(173,3,'admin:fingerprint','delete'),(174,3,'admin:overtime','view'),(175,3,'admin:overtime','approve'),(176,3,'admin:overtime','report'),(177,3,'admin:leaveform','view'),(178,3,'admin:leaveform','approve'),(179,3,'admin:leaveform','report'),(180,3,'admin:asset','view'),(181,3,'admin:asset','approve'),(182,3,'admin:asset','report'),(183,3,'admin:wfh','view'),(184,3,'admin:wfh','approve'),(185,4,'staff:dashboard','view'),(186,4,'staff:timesheet','view'),(187,4,'staff:timesheet:other','view'),(188,4,'staff:timesheet:other','lock'),(189,4,'staff:overtime','view'),(190,4,'staff:leave','view'),(191,4,'staff:asset','view'),(192,4,'staff:calendar','view'),(193,4,'staff:wfh','view'),(194,4,'admin:user','view'),(195,4,'admin:user','create'),(196,4,'admin:user','edit'),(197,4,'admin:user','inactive'),(198,4,'admin:group','view'),(199,4,'admin:group','create'),(200,4,'admin:group','edit'),(201,4,'admin:group','delete'),(202,4,'admin:project','view'),(203,4,'admin:project','create'),(204,4,'admin:project','edit'),(205,4,'admin:project','delete'),(206,4,'admin:analysis','view'),(207,4,'admin:analysis','report'),(208,4,'admin:object','view'),(209,4,'admin:object','create'),(210,4,'admin:object','edit'),(211,4,'admin:object','delete'),(212,4,'admin:category','view'),(213,4,'admin:category','create'),(214,4,'admin:category','edit'),(215,4,'admin:category','delete'),(216,4,'admin:fingerprint','view'),(217,4,'admin:fingerprint','edit'),(218,4,'admin:fingerprint','delete'),(219,4,'admin:overtime','view'),(220,4,'admin:overtime','approve'),(221,4,'admin:overtime','report'),(222,4,'admin:leaveform','view'),(223,4,'admin:leaveform','approve'),(224,4,'admin:leaveform','report'),(225,4,'admin:asset','view'),(226,4,'admin:asset','approve'),(227,4,'admin:asset','report'),(228,4,'admin:wfh','view'),(229,4,'admin:wfh','approve'),(230,5,'staff:dashboard','view'),(231,5,'staff:timesheet','view'),(232,5,'staff:timesheet:other','view'),(233,5,'staff:overtime','view'),(234,5,'staff:leave','view'),(235,5,'staff:asset','view'),(236,5,'staff:calendar','view'),(237,5,'staff:wfh','view'),(238,5,'admin:project','view'),(239,5,'admin:project','create'),(240,5,'admin:project','edit'),(241,5,'admin:project','delete'),(242,5,'admin:analysis','view'),(243,5,'admin:analysis','report'),(244,5,'admin:overtime','view'),(245,5,'admin:leaveform','view'),(246,5,'admin:wfh','view'),(247,6,'staff:dashboard','view'),(248,6,'staff:timesheet','view'),(249,6,'staff:overtime','view'),(250,6,'staff:leave','view'),(251,6,'staff:asset','view'),(252,6,'staff:calendar','view'),(253,6,'staff:wfh','view'),(254,7,'staff:dashboard','view'),(255,7,'staff:timesheet','view'),(256,7,'staff:overtime','view'),(257,7,'staff:leave','view'),(258,7,'staff:asset','view'),(259,7,'staff:calendar','view'),(260,7,'staff:wfh','view'),(261,8,'staff:dashboard','view'),(262,8,'staff:timesheet','view'),(263,8,'staff:overtime','view'),(264,8,'staff:asset','view'),(265,8,'staff:calendar','view'),(266,8,'staff:wfh','view');
/*!40000 ALTER TABLE `RoleClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `SwapDay`
--

LOCK TABLES `SwapDay` WRITE;
/*!40000 ALTER TABLE `SwapDay` DISABLE KEYS */;
/*!40000 ALTER TABLE `SwapDay` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `SwapDayRefer`
--

LOCK TABLES `SwapDayRefer` WRITE;
/*!40000 ALTER TABLE `SwapDayRefer` DISABLE KEYS */;
/*!40000 ALTER TABLE `SwapDayRefer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `SwapDayUser`
--

LOCK TABLES `SwapDayUser` WRITE;
/*!40000 ALTER TABLE `SwapDayUser` DISABLE KEYS */;
/*!40000 ALTER TABLE `SwapDayUser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `SystemRequest`
--

LOCK TABLES `SystemRequest` WRITE;
/*!40000 ALTER TABLE `SystemRequest` DISABLE KEYS */;
INSERT INTO `SystemRequest` VALUES (1,0,2,'2024-07-01 00:00:00',0,0,0,'2024-06-25 07:14:39'),(2,0,3,'2024-07-02 00:00:00',0,0,0,'2024-06-25 07:14:40'),(3,0,4,'2024-07-03 00:00:00',0,0,0,'2024-06-25 07:14:42'),(4,0,5,'2024-07-04 00:00:00',0,0,0,'2024-06-25 07:14:43'),(5,0,6,'2024-07-05 00:00:00',0,0,0,'2024-06-25 07:14:44');
/*!40000 ALTER TABLE `SystemRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Team`
--

LOCK TABLES `Team` WRITE;
/*!40000 ALTER TABLE `Team` DISABLE KEYS */;
/*!40000 ALTER TABLE `Team` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `TeamCategory`
--

LOCK TABLES `TeamCategory` WRITE;
/*!40000 ALTER TABLE `TeamCategory` DISABLE KEYS */;
/*!40000 ALTER TABLE `TeamCategory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `TeamObject`
--

LOCK TABLES `TeamObject` WRITE;
/*!40000 ALTER TABLE `TeamObject` DISABLE KEYS */;
/*!40000 ALTER TABLE `TeamObject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `TeamUser`
--

LOCK TABLES `TeamUser` WRITE;
/*!40000 ALTER TABLE `TeamUser` DISABLE KEYS */;
/*!40000 ALTER TABLE `TeamUser` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `Timesheet`
--

LOCK TABLES `Timesheet` WRITE;
/*!40000 ALTER TABLE `Timesheet` DISABLE KEYS */;
/*!40000 ALTER TABLE `Timesheet` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `TimesheetDetail`
--

LOCK TABLES `TimesheetDetail` WRITE;
/*!40000 ALTER TABLE `TimesheetDetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `TimesheetDetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `TimesheetReport`
--

LOCK TABLES `TimesheetReport` WRITE;
/*!40000 ALTER TABLE `TimesheetReport` DISABLE KEYS */;
/*!40000 ALTER TABLE `TimesheetReport` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `User`
--

LOCK TABLES `User` WRITE;
/*!40000 ALTER TABLE `User` DISABLE KEYS */;
INSERT INTO `User` VALUES (1,'demo0',NULL,_binary '',0,'0',5,NULL,1,'2024-06-25 07:12:40',1,'2024-06-25 07:12:40','demo0','DEMO0','0@ttdesignco.com','0@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEN0+WaQlqXQR73kYBOKx2pUmO0KUkA1rXkUBkegMMuCvxrQzKDpqkBppv25IXBWj+Q==','TFSFWMVTGDLTAMDDQMKUI7LKGSZSTXGY','19e3faa5-afab-46ee-baab-c830fbbd6737',NULL,0,0,NULL,1,0),(2,'demo1',NULL,_binary '',0,'1',5,NULL,1,'2024-06-25 07:12:41',1,'2024-06-25 07:12:41','demo1','DEMO1','1@ttdesignco.com','1@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHd0JpQ8Qkl/fovyIDZgGJV9VHS/6LtZ+bmNAfCquSAcE+4JWWV3GJnPUgifinEt0g==','ZNGDHHVJQRUVC4N4J7BQEOZNHXEEZOB4','36c3e070-8573-491c-9933-85635307770c',NULL,0,0,NULL,1,0),(3,'demo2',NULL,_binary '',0,'2',5,NULL,1,'2024-06-25 07:12:43',1,'2024-06-25 07:12:43','demo2','DEMO2','2@ttdesignco.com','2@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEI4TBm0YbXBsn95EySW4XVjSRNj08amoqO3qA/AaCwaeVqYxAaRSDmRkFciNSXutHw==','GVZUKVNNKCPIY66FVS3UXCRYVBVLAD3X','5cd069be-fa75-43a0-9354-23a51a95fceb',NULL,0,0,NULL,1,0),(4,'demo3',NULL,_binary '',0,'3',5,NULL,1,'2024-06-25 07:12:44',1,'2024-06-25 07:12:44','demo3','DEMO3','3@ttdesignco.com','3@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECkDVlnu4S+uUvqlt4gyCbLIAdWiJ6fWB2ZBXFHNvt1sijlgdFES1oazpEK7cjjkOQ==','J5UX5RLJ2UN2K25ZI3DHM4DQOGYI26MS','e38b170a-6b52-4158-8c63-ecf9ed6cd957',NULL,0,0,NULL,1,0),(5,'demo4',NULL,_binary '',0,'4',5,NULL,1,'2024-06-25 07:12:45',1,'2024-06-25 07:12:45','demo4','DEMO4','4@ttdesignco.com','4@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJnQx+SW0KAMF9CVz9KchqWmVf1QfDQ/wuBfo+BRSmGZZrdVBsiFCFRqbbPt0nIgPg==','4DBFCIPZQPYN6RYEKUHATGGQNDG5JWXK','c7045668-5068-4a98-b381-3dffb4b1e3f3',NULL,0,0,NULL,1,0),(6,'demo5',NULL,_binary '',0,'5',5,NULL,1,'2024-06-25 07:12:46',1,'2024-06-25 07:12:46','demo5','DEMO5','5@ttdesignco.com','5@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELkYGwaqXiehy8cn7BW7ZtQZOl8KcO+vBMM39sdCGEUjtNqwdmfnMPFPBQEWwk7gig==','S4KIMNI4UYOZJTPLXSRDJAKQLHH3VYOE','780c1f6f-f134-4e6d-b549-44de36b08214',NULL,0,0,NULL,1,0),(7,'demo6',NULL,_binary '',0,'6',5,NULL,1,'2024-06-25 07:12:47',1,'2024-06-25 07:12:47','demo6','DEMO6','6@ttdesignco.com','6@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEGcOkZjWRoqtC4sVqzw+3gC5z9xxPE0q+EuzAHdtNBKk3dlFvWH1XC8eTConFuSAmQ==','F54X64VWKNDGP34A2PEJIGWLGRCNRS4I','bce2df1c-7cac-44a9-95b5-b40faf18048a',NULL,0,0,NULL,1,0),(8,'demo7',NULL,_binary '',0,'7',5,NULL,1,'2024-06-25 07:12:48',1,'2024-06-25 07:12:48','demo7','DEMO7','7@ttdesignco.com','7@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFgdehu2g3/4iN0nYMtoyB+0lAI+wlQ2EhjWJisFMNcPF1tosZ+az+mmG0Ug1DELwQ==','Y725IH37GFE3PTIXLW3XTEI6PAHBLOIC','5893a1ae-803f-4aa4-8d48-93f3328f8fac',NULL,0,0,NULL,1,0),(9,'demo8',NULL,_binary '',0,'8',5,NULL,1,'2024-06-25 07:12:50',1,'2024-06-25 07:12:50','demo8','DEMO8','8@ttdesignco.com','8@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECUF+lYrrwhDWodMnpJzqhKcbI+KIor7a/znmfpl6UEKFGWpdQExjTSmGN+lwwG/kA==','MFSV3QHG6PDQFB4KEP5SAY6JML5X3EES','486dae8a-d89c-444e-8d0c-43e6537fb1d7',NULL,0,0,NULL,1,0),(10,'demo9',NULL,_binary '',0,'9',5,NULL,1,'2024-06-25 07:12:51',1,'2024-06-25 07:12:51','demo9','DEMO9','9@ttdesignco.com','9@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJ6IW5fqQQB62j1pe0+IFqhzcCTqrBE3eCXzeJ1gAtQWEPLaJjITtxpOPj1tWq6TzQ==','YWRGMN3IXXZGOFDQQDCTKQJSGMI62ERO','1632a7a1-5d8d-4068-99c0-d6bae3e2e24b',NULL,0,0,NULL,1,0),(11,'demo10',NULL,_binary '',0,'10',5,NULL,1,'2024-06-25 07:12:52',1,'2024-06-25 07:12:52','demo10','DEMO10','10@ttdesignco.com','10@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAO2xT2Nf4wDhHOuWU0CwSAhbhi0AGS4+qFT1w7PnXiscek9c3xNWwgvL6E9XN2SSw==','6VYOF7ITE6LH5LMLIRX4ZI5A4RSUPILW','1038dfa0-32fb-4035-af34-6c68a15ce846',NULL,0,0,NULL,1,0),(12,'demo11',NULL,_binary '',0,'11',5,NULL,1,'2024-06-25 07:12:53',1,'2024-06-25 07:12:53','demo11','DEMO11','11@ttdesignco.com','11@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAKMGiX/gwK2Ni8ZAvui5eKFxvnXAzOScHbEOb7gt0HxrDuapjZSa6PSD133M7AUVQ==','KS6VM65U4G7G4BKTM2O3C6ZUJLBY27JD','24ff5b6e-dc98-43ce-a142-6c9689e07e78',NULL,0,0,NULL,1,0),(13,'demo12',NULL,_binary '',0,'12',5,NULL,1,'2024-06-25 07:12:54',1,'2024-06-25 07:12:54','demo12','DEMO12','12@ttdesignco.com','12@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFof04ZnNiFYFB9Jj+BiTlApKrLvUmx3nAcrpDc57T0Kf56SUPTae2kFV39F667OtQ==','2ZXU6VLPL3R6R2MGJLSF3NTITM2BIANC','428b2f5e-e162-4611-aba5-59f1a2d1b7dc',NULL,0,0,NULL,1,0),(14,'demo13',NULL,_binary '',0,'13',5,NULL,1,'2024-06-25 07:12:55',1,'2024-06-25 07:12:55','demo13','DEMO13','13@ttdesignco.com','13@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEB/odhZ1Rl+cYg2yUG/0vX4sr33a1GVBrg6s4uy+U2Q+MbnMpNzrOrkiLaU1gk0SFg==','JJSTA27EFVJX2I3VJ37QFLJ5UGMXKBW5','8bd3c7bb-99fd-4c62-a6d6-a8a34c65efef',NULL,0,0,NULL,1,0),(15,'demo14',NULL,_binary '',0,'14',5,NULL,1,'2024-06-25 07:12:57',1,'2024-06-25 07:12:57','demo14','DEMO14','14@ttdesignco.com','14@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJeVLZ59EaArVKyh9uzy/6BFAMa28CkTN/vQSXmPauzfxDLSAB/L7kUytYHJq8agCQ==','7G6IQF2SKRWEHOX2ATFIJVC5XRF32OI5','f42253fa-ef8b-40f7-aebf-5b039c39733e',NULL,0,0,NULL,1,0),(16,'demo15',NULL,_binary '',0,'15',5,NULL,1,'2024-06-25 07:12:58',1,'2024-06-25 07:12:58','demo15','DEMO15','15@ttdesignco.com','15@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJxijrRJqYfYhwZ1pZLRckV72oSNiLtn1rPJN136Z72SsYUgRHdHGv/nB928SbAapA==','KKQLAMXQVCNPQHI2NRELSUX6PKUNOWOF','10d7a2d5-ae4f-42ae-a487-94e4fb80d848',NULL,0,0,NULL,1,0),(17,'demo16',NULL,_binary '',0,'16',5,NULL,1,'2024-06-25 07:12:59',1,'2024-06-25 07:12:59','demo16','DEMO16','16@ttdesignco.com','16@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMn4rGVb8dM/qdr4YCWmN+YByL0BMe3bTsOQlYh75nbtmGB9/+FpkwMA91U3rDfPRA==','GWW2VUXVAKQEKI6PNJUFREJJB6QANX54','e0c77554-bf59-4321-b8ef-9054e1ae13d4',NULL,0,0,NULL,1,0),(18,'demo17',NULL,_binary '',0,'17',5,NULL,1,'2024-06-25 07:13:00',1,'2024-06-25 07:13:00','demo17','DEMO17','17@ttdesignco.com','17@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEGGseW9+tHw+tuuT0XD3n/hKWrSFsgApNFOjRYRNFeBmO0Xd3ZvUXYs7cqw3sGrWsw==','Q7CPYTK76DBZIRNASWXCM33LKZHR4ACT','5e3c13f6-adf1-40ce-b967-93f3773580ab',NULL,0,0,NULL,1,0),(19,'demo18',NULL,_binary '',0,'18',5,NULL,1,'2024-06-25 07:13:01',1,'2024-06-25 07:13:01','demo18','DEMO18','18@ttdesignco.com','18@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFgIgOQcI0iKanMzeqKmR9Z28VOF4RTFv8QYVb9EFuHxqftUpYDhji908uDp9KD1BQ==','6DGMP5HNDV2F2E5VWUTUTAWSMVZ2FHD2','e62cb3a3-b968-4cba-bf74-d5ab029ffd04',NULL,0,0,NULL,1,0),(20,'demo19',NULL,_binary '',0,'19',5,NULL,1,'2024-06-25 07:13:02',1,'2024-06-25 07:13:02','demo19','DEMO19','19@ttdesignco.com','19@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKzc5o1KmOyF/oI4M7uwNQ1OYmGSagJe9Y39HcMtdT+z5LCffQea/BjHxLS2g5hbMg==','3V2GB7UHGC5RAJYVUHAI5KLUK5QZHMHE','fa2486aa-97e7-409e-9940-381640e3ec3e',NULL,0,0,NULL,1,0),(21,'demo20',NULL,_binary '',0,'20',5,NULL,1,'2024-06-25 07:13:04',1,'2024-06-25 07:13:04','demo20','DEMO20','20@ttdesignco.com','20@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEOQo9JZUA1KQn5jnkh7uWYl4GYITusZQy/YvCoMWfyHEdBltt9zsqxRheq4JR7KiZw==','T5EEMLJEEICLX7ZA5CIZRHDBYB5QFO6B','5ca7d09a-837d-444f-9c2c-15a812680315',NULL,0,0,NULL,1,0),(22,'demo21',NULL,_binary '',0,'21',5,NULL,1,'2024-06-25 07:13:05',1,'2024-06-25 07:13:05','demo21','DEMO21','21@ttdesignco.com','21@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAa/M/a2JMHf3h0x938A1MF9M8so+uZoxRzxloaM4MC/kg0Zlh5wPYIWfO28jk0vwA==','URXDKHXU52GOWACPJKTRR5YZOGB7DYL4','9737cd7a-deaa-4772-8389-8c80e1c5a479',NULL,0,0,NULL,1,0),(23,'demo22',NULL,_binary '',0,'22',5,NULL,1,'2024-06-25 07:13:06',1,'2024-06-25 07:13:06','demo22','DEMO22','22@ttdesignco.com','22@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEI6fuQJAKe9WzOW6ktTyVnLpZqPbQSovR/3xm33LiY3kc2EvdDdNze0h6SxA/iQmSQ==','LFJRW4Y733QKLPXYT44WSFK2YVAHK333','3eb7665d-b6a3-4241-98bf-0590fb54f2ff',NULL,0,0,NULL,1,0),(24,'demo23',NULL,_binary '',0,'23',5,NULL,1,'2024-06-25 07:13:07',1,'2024-06-25 07:13:07','demo23','DEMO23','23@ttdesignco.com','23@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJYIHD7livYDtgmhMRESxOOTj7io1YOh8mbxjP9r1gK3rxkXTxv42/7TWswBdGfEDg==','QAMM7YZN2NJFYLEPYBSS5ZJJ2DOXX33P','1e309966-5122-4c6c-b1c8-51337e046d74',NULL,0,0,NULL,1,0),(25,'demo24',NULL,_binary '',0,'24',5,NULL,1,'2024-06-25 07:13:08',1,'2024-06-25 07:13:08','demo24','DEMO24','24@ttdesignco.com','24@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAENCUs68oS4hakMw+BaBvfckqTOuzPZL0NWvZBfjWEdLDvkNBznVdcwwPfUY+SIf/8A==','3ZJCDIOU5FXVJ7XJ6LC5QEAYXNNA4HXT','2755e6b9-626c-4a73-a87a-7ee10238dea2',NULL,0,0,NULL,1,0),(26,'demo25',NULL,_binary '',0,'25',5,NULL,1,'2024-06-25 07:13:09',1,'2024-06-25 07:13:09','demo25','DEMO25','25@ttdesignco.com','25@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMjLgsvKmySkReei+/cZ7CPQVbD9U4YfjTulXvI52NoTg4B4ldoQ3L4kGUjT5Y0Lvw==','HP563QEFPQ26P4E575FIF64VL25EKCMP','2ccfeb74-a36d-4de0-92d5-6402fb981901',NULL,0,0,NULL,1,0),(27,'demo26',NULL,_binary '',0,'26',5,NULL,1,'2024-06-25 07:13:11',1,'2024-06-25 07:13:11','demo26','DEMO26','26@ttdesignco.com','26@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEECxRa4EszblRnH3oE9sa8zyWB2zizdXKrPbggDVuKlDY8WTY2CN+Ou0EGEY0nxumA==','K5RI77M2LEZHROWU2GROWF52LODJKRN3','0cfd59d1-9e40-4fb1-bee0-bc34cb396e3f',NULL,0,0,NULL,1,0),(28,'demo27',NULL,_binary '',0,'27',5,NULL,1,'2024-06-25 07:13:12',1,'2024-06-25 07:13:12','demo27','DEMO27','27@ttdesignco.com','27@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEGu6CapwVbcwgB8+k+gDteI8IZqCTDuK3RuFXMaKdkJAQGsgewU8YNB/8F2a8LOKTA==','XBTEEHP4S4XR6Z6SZHYKIJGUU2RMRRAP','765d5bff-5811-4a71-92dc-bdd6eae65bcf',NULL,0,0,NULL,1,0),(29,'demo28',NULL,_binary '',0,'28',5,NULL,1,'2024-06-25 07:13:13',1,'2024-06-25 07:13:13','demo28','DEMO28','28@ttdesignco.com','28@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEDzI8lC5oV3A950r2Y4OvJwnzwaV1Si2ZYUtUC5ZNth2Ye25gAA4VdYfPcs3DHqr1A==','VN2N4TYTAKIULZV4XWBOPVPXIHNN7GFQ','689b7eb6-f6c6-44b9-8d94-639548367dee',NULL,0,0,NULL,1,0),(30,'demo29',NULL,_binary '',0,'29',5,NULL,1,'2024-06-25 07:13:14',1,'2024-06-25 07:13:14','demo29','DEMO29','29@ttdesignco.com','29@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMrwxjmlm7Wya63AnX3zrXjxm/BUkL8J+AsAQGse4F53IhHzsGmKKRTi1AMTPQprkw==','Q6HZ7QMXAQNEXPM3QAXQBMY23JUXJKOP','26f9324c-b212-4c3c-a180-8c23ffcf09b5',NULL,0,0,NULL,1,0),(31,'demo30',NULL,_binary '',0,'30',5,NULL,1,'2024-06-25 07:13:15',1,'2024-06-25 07:13:15','demo30','DEMO30','30@ttdesignco.com','30@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEONaIULQMWfTS1DpauVbNjY1XD3ZCC5+/r5LnvJrUlD4+jVRCPrh0dE0/0I1+s28pw==','NK3LLHW5BH4HB5Y35IDO6ME7EKY3Z475','1596f7d1-ed02-46fa-9df3-961fc39657db',NULL,0,0,NULL,1,0),(32,'demo31',NULL,_binary '',0,'31',5,NULL,1,'2024-06-25 07:13:16',1,'2024-06-25 07:13:16','demo31','DEMO31','31@ttdesignco.com','31@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELJy9edzr8aQSRHOigSQmkW4BYGmum/J9d4CxGPH3RSX8y6lnwxtYZzxB3XaRgFnAQ==','I42A3CQMIJ73OTXH6S22J5ZMKTQ7QYOA','14919e3d-ebde-41f8-bd1d-f15391b12812',NULL,0,0,NULL,1,0),(33,'demo32',NULL,_binary '',0,'32',5,NULL,1,'2024-06-25 07:13:18',1,'2024-06-25 07:13:18','demo32','DEMO32','32@ttdesignco.com','32@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEB+yuEtxNKZnUZ9aUd8jgZOCGHsdiafij/BTvZFC2kHBfo1QmlVaVtRR/3a/3UkNnQ==','A5SW5SRYX6UBGHSN6HB7C5WCNONE5LBG','25c93e2b-77f0-4b2d-9f3c-c3944daa1795',NULL,0,0,NULL,1,0),(34,'demo33',NULL,_binary '',0,'33',5,NULL,1,'2024-06-25 07:13:19',1,'2024-06-25 07:13:19','demo33','DEMO33','33@ttdesignco.com','33@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEF0WcyZV3To2Ahc70eU3ldB8nr3NzYn5QWzqMCUy+qjZqK6QYnaWidwavVVnrLYyOg==','6XEBJ2NTO3XJMOCHICP7RAFHCDFMPGVV','a98dfb97-8d13-4441-ad52-a34e164b116a',NULL,0,0,NULL,1,0),(35,'demo34',NULL,_binary '',0,'34',5,NULL,1,'2024-06-25 07:13:20',1,'2024-06-25 07:13:20','demo34','DEMO34','34@ttdesignco.com','34@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEkewUKc1UA6LR9u5aYl5+S02TgEJfB8FcRHY0xEUMtlp3ylYAQOaGJJWilk8Me3Og==','RZP2FCGYFUZJNR5EES6VV7MCMFOCTIV3','71ece990-830a-44fb-8862-facb0455f2b7',NULL,0,0,NULL,1,0),(36,'demo35',NULL,_binary '',0,'35',5,NULL,1,'2024-06-25 07:13:21',1,'2024-06-25 07:13:21','demo35','DEMO35','35@ttdesignco.com','35@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEsvRQVnDrXGWV6Xd98Ww9oIU8GDHoRZhCZ9YIEDajuTtG8UpYwTaSAvUwwQDWYBxA==','QYNGA7SQ32CT2J6I5HGP3UP7WMAT5NFO','bc666e15-9fe4-435b-95f7-7ef82e02eb4a',NULL,0,0,NULL,1,0),(37,'demo36',NULL,_binary '',0,'36',5,NULL,1,'2024-06-25 07:13:22',1,'2024-06-25 07:13:22','demo36','DEMO36','36@ttdesignco.com','36@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEDX2z0WT3+ORKgdYQUC6SCCFocNxbRULX3op9YsAYVnM1lMevJGjKjdQdYUCp2HICg==','BTTBXT3XLMWUF5JKN2POF7XZADOCS7JZ','7a908c67-198b-47d5-a10a-c6ca4094693a',NULL,0,0,NULL,1,0),(38,'demo37',NULL,_binary '',0,'37',5,NULL,1,'2024-06-25 07:13:24',1,'2024-06-25 07:13:24','demo37','DEMO37','37@ttdesignco.com','37@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEOpLjm1lnW/ZAlE+7O1SljXOpgwtYlRep4Wk/Kho1KJGXZeW2dUvIdd+ww1gkODHA==','Y25ZETYH5R7UIQAFBZPZNBKMXE5R74O2','0e0daadc-6d7f-4def-b35d-dcebe80d4b84',NULL,0,0,NULL,1,0),(39,'demo38',NULL,_binary '',0,'38',5,NULL,1,'2024-06-25 07:13:25',1,'2024-06-25 07:13:25','demo38','DEMO38','38@ttdesignco.com','38@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAloV0Pyyz+7LdRA/VXYmcvGykj43IZlWiGnlomZB1+6CKIDLmFD1X4kwg5EzBUPVQ==','XD2O5FLZWMK3QEMPDM55YGLIIFIZWNW6','4e41c77d-1da4-4d6d-ba26-c872c3bcfe1d',NULL,0,0,NULL,1,0),(40,'demo39',NULL,_binary '',0,'39',5,NULL,1,'2024-06-25 07:13:26',1,'2024-06-25 07:13:26','demo39','DEMO39','39@ttdesignco.com','39@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHh7GQG21UhHH/kWHbZWO3LaF2SlIhoHgMQ9bLyt1Y4Lb9Aqw0MHbklENmA7p0HodA==','HM5B7PBJNPN7ZLLAN5N3TQSPKWIGBA4U','f878b404-9285-4afe-b7c6-0af69e33ee98',NULL,0,0,NULL,1,0),(41,'demo40',NULL,_binary '',0,'40',5,NULL,1,'2024-06-25 07:13:27',1,'2024-06-25 07:13:27','demo40','DEMO40','40@ttdesignco.com','40@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEDHesdMKrg5jlZGUEgF3MXiVJAkOhSiOTqoijoXk72lVXWspdmYpR76Q5sR0pvI/5Q==','R4X545PBWVXVF5NIMMXQXJSCJ4E63GGC','ce2f3cf0-11c8-490e-8813-763aecce0971',NULL,0,0,NULL,1,0),(42,'demo41',NULL,_binary '',0,'41',5,NULL,1,'2024-06-25 07:13:28',1,'2024-06-25 07:13:28','demo41','DEMO41','41@ttdesignco.com','41@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMNH9MS8sUnBAkqOcRHynIc4ktl7d/NSW4fNQ1YhkpjhBF7Cj2O5LdxRgNKZJ63PsQ==','5UPPVDE3TMLYDEPGOHKSDXFBUVIOYBSO','595e67b3-4846-43a5-bd7c-0c536ffc51d8',NULL,0,0,NULL,1,0),(43,'demo42',NULL,_binary '',0,'42',5,NULL,1,'2024-06-25 07:13:29',1,'2024-06-25 07:13:29','demo42','DEMO42','42@ttdesignco.com','42@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEA5axlCSj7p22IR/PXt4WSwScoD1PKLQgVkHMiEod5+PDEd/qdX1XZNN+kg/F66x/A==','OLVMZZDKNSC2HQZ4IID32HOBUZHT2AAZ','31698257-5786-4ea0-8226-b35625b0eb7f',NULL,0,0,NULL,1,0),(44,'demo43',NULL,_binary '',0,'43',5,NULL,1,'2024-06-25 07:13:31',1,'2024-06-25 07:13:31','demo43','DEMO43','43@ttdesignco.com','43@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEI5s8Kw4MajFBOAKQ0kIQ/fAlDpu9/rN9qSN57ACIInZABm1ZMC2Z5Tqy0I8ovZelA==','QH3XT7WXFFSCTDQYC7FEACKNMDQB4BHQ','1aeb8321-b101-44e3-b3a9-3f3d8f2cffe5',NULL,0,0,NULL,1,0),(45,'demo44',NULL,_binary '',0,'44',5,NULL,1,'2024-06-25 07:13:32',1,'2024-06-25 07:13:32','demo44','DEMO44','44@ttdesignco.com','44@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMzVYyQ2htB15Mm+5VjWELVpfgfttgY98toql12Vl9QGKokoJl1JoX2cuexsRaE+LQ==','IBM7ZHDXL3XLIWMHEOHXNQ3YQLKZP6OS','5145d506-5caf-4bc9-b9c2-de387bd46828',NULL,0,0,NULL,1,0),(46,'demo45',NULL,_binary '',0,'45',5,NULL,1,'2024-06-25 07:13:33',1,'2024-06-25 07:13:33','demo45','DEMO45','45@ttdesignco.com','45@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPpPkpxf1SET6ip8Bn1oGiOnFIgTChZhVlCNAGbSGYSpbahvLnB7DxNqQjpNupbf4A==','JYPG5YDCUCFV7JKD3YYQ5YL5X4SDQIUE','c40005b3-81c1-46a1-96f7-b5104b5a40c7',NULL,0,0,NULL,1,0),(47,'demo46',NULL,_binary '',0,'46',5,NULL,1,'2024-06-25 07:13:34',1,'2024-06-25 07:13:34','demo46','DEMO46','46@ttdesignco.com','46@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJ2SYIpOi3HHsZRkTDekh/mz7YNvTKAI8rDgVLKv7z4oES4OVsp8XgNGgV/Ds22E+w==','B4CYV5HQRVARPSKGGEREZ44FLIQEFKUU','7c0197c7-cb06-491e-962d-01eaef397aaa',NULL,0,0,NULL,1,0),(48,'demo47',NULL,_binary '',0,'47',5,NULL,1,'2024-06-25 07:13:36',1,'2024-06-25 07:13:36','demo47','DEMO47','47@ttdesignco.com','47@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECuNkBwVkXrnVJKkSEyPXwAGrivKxo2mEIKhb/e7Fbmj0aqObFipWG/GxGnViFW0wA==','4CDMR2DCZQLIVF6252Y45W4D34XPPS54','b5390877-ce8e-4e43-94c3-e0cb059c96c7',NULL,0,0,NULL,1,0),(49,'demo48',NULL,_binary '',0,'48',5,NULL,1,'2024-06-25 07:13:37',1,'2024-06-25 07:13:37','demo48','DEMO48','48@ttdesignco.com','48@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEImiCvERb9NYYIozBUihsVJy+IPCLuTlfeJZs2sPUR6vM1xUM5ArEXzhOhICXcX5wg==','PWHIVKQSNDV2QTHL62BJDTMWJZNENOKI','12d88c1f-1d00-477b-9cfc-de2c8624f2ea',NULL,0,0,NULL,1,0),(50,'demo49',NULL,_binary '',0,'49',5,NULL,1,'2024-06-25 07:13:38',1,'2024-06-25 07:13:38','demo49','DEMO49','49@ttdesignco.com','49@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEBuAzqZAP++w0bw8MgBmMqbWGZ6G4PtPnVUxiak6vqFXJbL/Hj+ftwxCG5s3JGhNCQ==','3FUOHBUVD4ABP552ZCJ3UNQB7P5M6OLF','72d2cef8-1d86-4993-bde0-6c66a50e32d5',NULL,0,0,NULL,1,0),(51,'demo50',NULL,_binary '',0,'50',5,NULL,1,'2024-06-25 07:13:39',1,'2024-06-25 07:13:39','demo50','DEMO50','50@ttdesignco.com','50@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEIWJ2R5sPxAqL2Iw0HwxiT43q9PY9aMzwILTHb7wIbPAaygByj6I8O7eZsNQPdQOTw==','PTBQ2QJVUZHQBLRRZDCONA3QZ2QVGCFP','f9f766dc-7e24-4ff0-b4b4-eac20f6f6c14',NULL,0,0,NULL,1,0),(52,'demo51',NULL,_binary '',0,'51',5,NULL,1,'2024-06-25 07:13:40',1,'2024-06-25 07:13:40','demo51','DEMO51','51@ttdesignco.com','51@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJX2EvmWdEUFPPe7rVahrdRgaXeGJz8ceNKUVQnNTtv9knv5tbjzR2ffy6xuXBOCkQ==','HVH5KCYD7OHZ36HE2CZ7KN4K23XVEUQN','3d462984-5513-4157-834e-7a90b59be9f1',NULL,0,0,NULL,1,0),(53,'demo52',NULL,_binary '',0,'52',5,NULL,1,'2024-06-25 07:13:41',1,'2024-06-25 07:13:41','demo52','DEMO52','52@ttdesignco.com','52@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHiPReEoprW6X6pdrp+M7007abnQXXF0pu81Hw0hxHTBqcVS+2ue4/7pE38ZyNVdhQ==','EEPGFUBEE6MEOJQ27DC3CLFWD6A552XA','7d76a795-2ef6-4a92-ae16-03db6b6b7260',NULL,0,0,NULL,1,0),(54,'demo53',NULL,_binary '',0,'53',5,NULL,1,'2024-06-25 07:13:43',1,'2024-06-25 07:13:43','demo53','DEMO53','53@ttdesignco.com','53@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPQYhN0/DURMFvnz/RLsleVR0MlZOzOe3vJAN+hnD8UrROZnPhuNLYTk5m7iZWXbhw==','RC2EENN2ZLESJPNXH7V6QIENHR6YKKT3','c76a3910-eb82-4523-b60d-c6879ce94777',NULL,0,0,NULL,1,0),(55,'demo54',NULL,_binary '',0,'54',5,NULL,1,'2024-06-25 07:13:44',1,'2024-06-25 07:13:44','demo54','DEMO54','54@ttdesignco.com','54@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPRYdaaHJ+D7CtBWoNfy6MswdWpkbbCyN1WpprOO0hqXqfAs3hkVoLHfHgDybK3pig==','OVWLJFKMXBCERDSGBEGKRCAKO2PORB66','5710a043-5cb1-4d20-9228-4922fffb9b55',NULL,0,0,NULL,1,0),(56,'demo55',NULL,_binary '',0,'55',5,NULL,1,'2024-06-25 07:13:45',1,'2024-06-25 07:13:45','demo55','DEMO55','55@ttdesignco.com','55@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHw7DipgnQKYRsAdeKVXm/nxtLw/P5oe52J108nJ4m0ottk4ZbWQu9FnynHFtJAbFA==','L6YNCOHD24QQ5WGRGZAYEGJAGQRJEQEO','8e9c0c97-ecc1-435a-8d89-408c99ea3b7c',NULL,0,0,NULL,1,0),(57,'demo56',NULL,_binary '',0,'56',5,NULL,1,'2024-06-25 07:13:46',1,'2024-06-25 07:13:46','demo56','DEMO56','56@ttdesignco.com','56@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEP/A1fv1xDqIwM5AXC36d+ZIyMGzdu/JAsAbVxSO2FoB6+ejUdYzca9eTjONcgPSPQ==','PIUJVD5NA5RGUTMWYXDGXSHG5XRPVIOA','fdc1e885-e2c3-42cf-996e-8ff336adfa4a',NULL,0,0,NULL,1,0),(58,'demo57',NULL,_binary '',0,'57',5,NULL,1,'2024-06-25 07:13:47',1,'2024-06-25 07:13:47','demo57','DEMO57','57@ttdesignco.com','57@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEw9UGdXbt6b2q8CSdFUtXEBEbEGF7VETJJk9oLRfgX0GwBeHVhcwbzDAD3e/yePMg==','VXOE326FM42XIBOKQH2UGYNCPUXUZMLF','93a426f9-2e53-469c-8c3c-e5fa1949da01',NULL,0,0,NULL,1,0),(59,'demo58',NULL,_binary '',0,'58',5,NULL,1,'2024-06-25 07:13:48',1,'2024-06-25 07:13:48','demo58','DEMO58','58@ttdesignco.com','58@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEM8HW9ABhOJricp0Jo2XU7IpmVjT5hcGWpxj9/JGs3LrUZUeKzDXqgwOhn2PhKx1TA==','R4BMSC4XFWOHWZQ56S6S2XJUT27SOZPS','0f833809-b33e-41be-b2dd-f0426017f990',NULL,0,0,NULL,1,0),(60,'demo59',NULL,_binary '',0,'59',5,NULL,1,'2024-06-25 07:13:50',1,'2024-06-25 07:13:50','demo59','DEMO59','59@ttdesignco.com','59@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECX8FS6yPKTEXLjtdkGOYZdiabAmVbmZHTDw0z+TcubwnhZfrHrX4ZefLaXLv/j6YA==','KC3QEWWGACLSNLSV7NISJ4NO2DINQGP4','b94b5676-9dae-40d7-bd2d-203bfa0b19cf',NULL,0,0,NULL,1,0),(61,'demo60',NULL,_binary '',0,'60',5,NULL,1,'2024-06-25 07:13:51',1,'2024-06-25 07:13:51','demo60','DEMO60','60@ttdesignco.com','60@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELJeEku0/M4m9oB9spr1ffan3nnE1qGB8uYrKSUh7cZKvTkE+jXBM8pRRUi2U5APNQ==','YZS4H2OFBTAPFXFU6T2WIWV74NY6HNQC','c91e58f6-deb2-4012-84f7-a6e7145c2330',NULL,0,0,NULL,1,0),(62,'demo61',NULL,_binary '',0,'61',5,NULL,1,'2024-06-25 07:13:52',1,'2024-06-25 07:13:52','demo61','DEMO61','61@ttdesignco.com','61@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEANupeyHnrKkGgeDR/9EkIARgQmbnn32b075azgo8XkvV+eIDBkXp6SUlLy6SdnQuQ==','RZLYFNHG3QXOMGMHNXETELGZSRNAGI7R','c41ea106-073b-4ece-b61d-734544896d8d',NULL,0,0,NULL,1,0),(63,'demo62',NULL,_binary '',0,'62',5,NULL,1,'2024-06-25 07:13:53',1,'2024-06-25 07:13:53','demo62','DEMO62','62@ttdesignco.com','62@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELnAq54wPJI99BZeWEGoHKzL26nwtwvSwACqQ6dh7iJTjPmc0H4iTd11nFM5rWfvNQ==','6K3HPLNLNZJQFAF4Y7JQQV5I74VFGFCJ','f7fbe335-d5ab-4ed3-9505-a1e361a6c668',NULL,0,0,NULL,1,0),(64,'demo63',NULL,_binary '',0,'63',5,NULL,1,'2024-06-25 07:13:54',1,'2024-06-25 07:13:54','demo63','DEMO63','63@ttdesignco.com','63@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEG29/CXcqVPp35qiFe2PgdeJMrlsI+0PCgr0G3YcEdJD0jSvGVSXM2gH+xTenGf7sQ==','RNNNGPHMC4ZDFCUP4TRTAIKPAOX5U4E7','a43c18ff-4e1e-4b16-a11b-b1ca3f91ee00',NULL,0,0,NULL,1,0),(65,'demo64',NULL,_binary '',0,'64',5,NULL,1,'2024-06-25 07:13:55',1,'2024-06-25 07:13:55','demo64','DEMO64','64@ttdesignco.com','64@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEBhabjtUMbGJxyjKdedwWOir/uPy223VdIny2IrtzSfxQLk0YL27miFKi7hE1M3qkw==','NGZLM3Z2B5NXBIPR27TK5ZYXAYAGR2NM','5102e0ff-7665-4b9e-a992-50f1799120d1',NULL,0,0,NULL,1,0),(66,'demo65',NULL,_binary '',0,'65',5,NULL,1,'2024-06-25 07:13:57',1,'2024-06-25 07:13:57','demo65','DEMO65','65@ttdesignco.com','65@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPlteBCc3WOV+7a19IKnzfXxLI/dYK4DWw9vSHwz5lUrC5iyLQCNRKaUmn2kB9ZUHA==','DNNWESNDDCOFWCV5B7WIBBDJLGKNNCKS','6be3b418-c68d-4ee5-a5c2-c8eceac70319',NULL,0,0,NULL,1,0),(67,'demo66',NULL,_binary '',0,'66',5,NULL,1,'2024-06-25 07:13:58',1,'2024-06-25 07:13:58','demo66','DEMO66','66@ttdesignco.com','66@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEI00o1g1tWT3zn/5K3F35IEV5et68mUxTetFYa3kFmm4TA0q07YhWelA0BzdVFRi4w==','DEHJZ5NJG24NCSUAKXVYFPISCW3L7KGE','4e4bb4ed-19b7-4baf-94be-4578d3e6cd84',NULL,0,0,NULL,1,0),(68,'demo67',NULL,_binary '',0,'67',5,NULL,1,'2024-06-25 07:13:59',1,'2024-06-25 07:13:59','demo67','DEMO67','67@ttdesignco.com','67@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEO/5GPJNLo4EePnM2dbgx2WU4b/AGzYUhshUqbZkA/JgWM0k/EWoW3tgWtSJ7UmRQQ==','ZN6VI2OCSANOTV5GIL5KD7JD2LTNBPMA','4af71746-0e4a-49f3-90cf-dad8b636bc15',NULL,0,0,NULL,1,0),(69,'demo68',NULL,_binary '',0,'68',5,NULL,1,'2024-06-25 07:14:00',1,'2024-06-25 07:14:00','demo68','DEMO68','68@ttdesignco.com','68@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEcgomKOSYwoNN0dfpH+VtEIA3NhvYTgp5BldXhZOHbs9oUJ1fJgq+lf9lkWJW+Mfw==','A6DH3UWYUOA5ZUOVRVZBILO66QAL5KNY','36f49939-bf35-47c6-914c-c19a36150cd0',NULL,0,0,NULL,1,0),(70,'demo69',NULL,_binary '',0,'69',5,NULL,1,'2024-06-25 07:14:01',1,'2024-06-25 07:14:01','demo69','DEMO69','69@ttdesignco.com','69@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKYak8m34FTafghnV1hNX3R3tgcq+q7EfuYIiElo00umllBg3sBjZQ3p1Vcst0rFyg==','S4LP4IENMVIBLOQQLDMRALRL7F5W7A42','0a7b2206-f9b9-4c0e-a971-5e4c06a03a7e',NULL,0,0,NULL,1,0),(71,'demo70',NULL,_binary '',0,'70',5,NULL,1,'2024-06-25 07:14:02',1,'2024-06-25 07:14:02','demo70','DEMO70','70@ttdesignco.com','70@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECjxG+jI1P5WRb1UBHHcV0zPUg/odVA6Oxan7y0sWKDd+8vjOQ4050uSj7JBPJc0BQ==','OOV756K523HHN5LJK4JH6DLRTBFIOFLN','68d10e2d-da2e-45e4-81f0-c10e3bdf5800',NULL,0,0,NULL,1,0),(72,'demo71',NULL,_binary '',0,'71',5,NULL,1,'2024-06-25 07:14:04',1,'2024-06-25 07:14:04','demo71','DEMO71','71@ttdesignco.com','71@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEOMHWlvHKgrsOTnnSIZ3C62tQIorP71dX3ABxUBpvhtEidky5tGAqr11pFlV59NHhg==','FQNLSGQZP52WMUOHEGUOHFROPI3HD6VL','4c45031d-2e86-41a9-910c-33b759dc0f26',NULL,0,0,NULL,1,0),(73,'demo72',NULL,_binary '',0,'72',5,NULL,1,'2024-06-25 07:14:05',1,'2024-06-25 07:14:05','demo72','DEMO72','72@ttdesignco.com','72@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFRWElLJrZBic0E38qt6udcilX05m7cKQ04eF6iil1ayxVRjdTdHNgms2YmFLVpbrQ==','GMJU4EOZFQU7BSTBFHPLOPXYIFQN5RTT','72a6658a-a01f-46ac-b21f-d3b05ed9e710',NULL,0,0,NULL,1,0),(74,'demo73',NULL,_binary '',0,'73',5,NULL,1,'2024-06-25 07:14:06',1,'2024-06-25 07:14:06','demo73','DEMO73','73@ttdesignco.com','73@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAENl5gy/4w6m36+xjkk+xICdoC1aTk2d/DGu0Db40guqEjibNQmK/J2ROwRI3vw1gkA==','OMRU6M4F6PV5FQFPTN7GTK2PFHOGIC3T','faaaa225-f58c-49d0-9233-8b51106be88f',NULL,0,0,NULL,1,0),(75,'demo74',NULL,_binary '',0,'74',5,NULL,1,'2024-06-25 07:14:07',1,'2024-06-25 07:14:07','demo74','DEMO74','74@ttdesignco.com','74@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELXZNLKqzDNrqNKmhSxO21w5kGOUm3MP0hhaEIvUr29aGIWIt69/jSsnVNHDe8oiyw==','IZXKK7JZMYVUPPCBQRPBGUIYJ76JXZRA','3a4b4ada-4603-4b74-88b3-22093a60d0de',NULL,0,0,NULL,1,0),(76,'demo75',NULL,_binary '',0,'75',5,NULL,1,'2024-06-25 07:14:08',1,'2024-06-25 07:14:08','demo75','DEMO75','75@ttdesignco.com','75@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEO7Zz+F2uCTDissNP0HXCvmeVZ3fXTsm7c5ggvYi782Jc3JHAOk4hSSTWPoAhpul8g==','VOPTIIRL7TMW7TJFVQ5FYHZBPPAOFCRU','63a50763-5ecd-4bbb-bafe-cd4e068ac2d5',NULL,0,0,NULL,1,0),(77,'demo76',NULL,_binary '',0,'76',5,NULL,1,'2024-06-25 07:14:10',1,'2024-06-25 07:14:10','demo76','DEMO76','76@ttdesignco.com','76@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEATf9edzPNN+nBEgD5w4qMuvEy+roj1bWzoyHmpw/olVBxJSfLuscy5BWp1iyze44A==','DWWPRDZTG5K76RW7YHDF4WJCT5RHPTPX','3612ffe8-1ef6-4a01-a654-68c50e7c72da',NULL,0,0,NULL,1,0),(78,'demo77',NULL,_binary '',0,'77',5,NULL,1,'2024-06-25 07:14:11',1,'2024-06-25 07:14:11','demo77','DEMO77','77@ttdesignco.com','77@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHyUlQf7U/I+MNJrBg5VXf7vurmXTohOZ4nmTbYnnOaOsq2R23+qkEQfn3g3OEBMGg==','CNROBHREXNJUMMJX26X7YUTO5EMGI36E','20350674-3064-482a-8c3a-ad1928566604',NULL,0,0,NULL,1,0),(79,'demo78',NULL,_binary '',0,'78',5,NULL,1,'2024-06-25 07:14:12',1,'2024-06-25 07:14:12','demo78','DEMO78','78@ttdesignco.com','78@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPxSe3rM1Ll23XsJjLEJjx2NHyieFl6VFKj/M+lKBsK1+KYaMUVfeObLDaEzqP/vdg==','WVEEVNN2FXM4SJ6GIPOUFDANVLIIJLTO','d61b8bdb-741b-4ce7-b7c0-ef9dd878460d',NULL,0,0,NULL,1,0),(80,'demo79',NULL,_binary '',0,'79',5,NULL,1,'2024-06-25 07:14:13',1,'2024-06-25 07:14:13','demo79','DEMO79','79@ttdesignco.com','79@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPFYsTW9CvtncFcyCV8V34SRzEq4e+fDyYR0rsF43T0nxVhyU5KQIeb4VUho+2p28g==','KJVNZUGOST2MNPIVQQKKMP6YHIMXJY6F','16892d3f-c5f7-414f-a925-4f30f3b02bf8',NULL,0,0,NULL,1,0),(81,'demo80',NULL,_binary '',0,'80',5,NULL,1,'2024-06-25 07:14:14',1,'2024-06-25 07:14:14','demo80','DEMO80','80@ttdesignco.com','80@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAmiLd2qRGCLUKL0WS4IWLBuaVtctxvEqxdSdKQCwctiZ88V0xSFFWEH+F5/R9+jYg==','XZPUDYXXHXTXUNZR2OSHE5SY6WKMLNJG','86603097-7a79-45dc-a347-18b800530f9d',NULL,0,0,NULL,1,0),(82,'demo81',NULL,_binary '',0,'81',5,NULL,1,'2024-06-25 07:14:15',1,'2024-06-25 07:14:15','demo81','DEMO81','81@ttdesignco.com','81@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAgIGtrJJMU9Xg6L0YQ3tAsKGhQeP4V1txuA6aIG15UgwNRGP7FRBQmrp2ecgZEojg==','E3E2NX2CXT3C4LGXCD6SQ2YPY3GY5YPF','458c4e92-cff2-4590-b80c-fcf1c578d7f9',NULL,0,0,NULL,1,0),(83,'demo82',NULL,_binary '',0,'82',5,NULL,1,'2024-06-25 07:14:17',1,'2024-06-25 07:14:17','demo82','DEMO82','82@ttdesignco.com','82@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAMsAKd+VTXUoKVTZGPicjumG5CIC+TW0kU/7BuqqmNSArbOzlADbx8ESsGHF2B5aQ==','H45LSAUPJQ6O7ESASZKXF4PHRTYPSQB5','3a519aa8-6efa-425f-9373-902b161577d6',NULL,0,0,NULL,1,0),(84,'demo83',NULL,_binary '',0,'83',5,NULL,1,'2024-06-25 07:14:18',1,'2024-06-25 07:14:18','demo83','DEMO83','83@ttdesignco.com','83@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECan/rPdu3sRthhkSO1AgxYNhiytNG0I6cvBDMBa1jp0UYVc0FJKifOAsaIurvJcRQ==','SE67OCEYWS5WYAKTZQIGEJ3WUPCDGXAL','e6d42783-31cc-473c-ab88-f64fa85e857a',NULL,0,0,NULL,1,0),(85,'demo84',NULL,_binary '',0,'84',5,NULL,1,'2024-06-25 07:14:19',1,'2024-06-25 07:14:19','demo84','DEMO84','84@ttdesignco.com','84@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEo57DrpWXNsvSV/1UXyjohpUgwd7DN6eX52166M0vWEdM+fIsC4ERI2HZo0lCzK1w==','VPM45DDGZYXMK3LZWFSXLVGS4YFJ772B','a9ea1a3c-30a0-4056-8cb6-59413f9513f2',NULL,0,0,NULL,1,0),(86,'demo85',NULL,_binary '',0,'85',5,NULL,1,'2024-06-25 07:14:20',1,'2024-06-25 07:14:20','demo85','DEMO85','85@ttdesignco.com','85@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEExpkr/UDrNritTGzFDFPPyjV1CU9ITALAGLO4LbyJKGsnGxV1sFknzWMzEUtDMtVA==','NMPTHE27IXFGTFRLOIIV23PJ5Z2GOCG4','af5c6f30-8a33-4eec-859d-997aa8be0604',NULL,0,0,NULL,1,0),(87,'demo86',NULL,_binary '',0,'86',5,NULL,1,'2024-06-25 07:14:21',1,'2024-06-25 07:14:21','demo86','DEMO86','86@ttdesignco.com','86@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEI7ObHcT6m5xm8DTTwYS8KSgcDQtmLhGCIoYHnrbU4xQQi7iMBJTrnTJche5yPbLiw==','XR3PR7BKBU5TXKPY3WFGH5VLQTJ52WGB','f2dea031-3155-48b2-8ad4-a29b3fb0ca9d',NULL,0,0,NULL,1,0),(88,'demo87',NULL,_binary '',0,'87',5,NULL,1,'2024-06-25 07:14:22',1,'2024-06-25 07:14:22','demo87','DEMO87','87@ttdesignco.com','87@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJSxlxoEEXerQ4ggw5OGIBhtif23WdW+m83NZ8riXmFTQjgV3LQb4hZL5z7VBszJlg==','Z2X3M6G3TZ5PIJQS2RJ7CE6HULMDFYNN','ea7623bd-4990-4386-a4b3-c6337ce39ad3',NULL,0,0,NULL,1,0),(89,'demo88',NULL,_binary '',0,'88',5,NULL,1,'2024-06-25 07:14:24',1,'2024-06-25 07:14:24','demo88','DEMO88','88@ttdesignco.com','88@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAENZCHuSeZrq7TzR2OgA728YguPdU83wB6PPWDfPiwE2EyTohx4j/u7TZbCiGlWFHng==','74LIEOPNYNQIH2AEVPXJPP624QYGUXKU','19378654-debc-4065-9bd7-dcddc0b9770d',NULL,0,0,NULL,1,0),(90,'demo89',NULL,_binary '',0,'89',5,NULL,1,'2024-06-25 07:14:25',1,'2024-06-25 07:14:25','demo89','DEMO89','89@ttdesignco.com','89@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEG1zQAJoKC5ga168cXaMNs53+wPSnVibDaMNr8T8miPCwSg+RDSnqpTJsNpPnllzag==','7CGE57KJ23UIZ7KA2YMRC554KQA6GVY2','68b65fd7-7d7d-48c5-b2be-52b7d0e56bbb',NULL,0,0,NULL,1,0),(91,'demo90',NULL,_binary '',0,'90',5,NULL,1,'2024-06-25 07:14:26',1,'2024-06-25 07:14:26','demo90','DEMO90','90@ttdesignco.com','90@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKJoQ1E38lJVCKffxLN8czTz+b9iz4ppLdOdKkFgFFpB2hx/V/WzhAguGBzqeM5l6Q==','CXYSFZW2LCP7TDXSWFVHQASPBB6AH2KN','0bb00751-4741-42ef-a487-c74918bb1446',NULL,0,0,NULL,1,0),(92,'demo91',NULL,_binary '',0,'91',5,NULL,1,'2024-06-25 07:14:27',1,'2024-06-25 07:14:27','demo91','DEMO91','91@ttdesignco.com','91@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFX0H3o8FgkfuorMenHfs1FnW/0m7OnC3oxjhWg2jrGGCrr2C5NzRfxaVfTp84+Jgw==','PLTCLALZLSGN3JHNXEUXFEF5GBP4NME2','6b0dd7aa-c3c2-42d7-ac56-f9ec6baa635c',NULL,0,0,NULL,1,0),(93,'demo92',NULL,_binary '',0,'92',5,NULL,1,'2024-06-25 07:14:29',1,'2024-06-25 07:14:29','demo92','DEMO92','92@ttdesignco.com','92@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELzhnf/sYNZXZ/3dnvdi1s55Yez/9YWmO4oW1ZBUo2XjkBgmspoIVbVGjUial3C2vw==','Z2JJQT7BQLG6PFKW65QFWQAVQ3UFSQ6D','be5c7ca8-b434-4cca-88ae-74b71b2cdfd5',NULL,0,0,NULL,1,0),(94,'demo93',NULL,_binary '',0,'93',5,NULL,1,'2024-06-25 07:14:30',1,'2024-06-25 07:14:30','demo93','DEMO93','93@ttdesignco.com','93@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELhhIqDodIq6h4QTOn6XYMcDv4pCCLxj2caiGXMqdl3J8xEY5VXtsEPErflU6Bi1gA==','YBWRJ3XKHKJNIWNYXOGVKNL4CKQQGQPA','32d5b5a7-e6ca-44f2-9228-02b006f62af6',NULL,0,0,NULL,1,0),(95,'demo94',NULL,_binary '',0,'94',5,NULL,1,'2024-06-25 07:14:31',1,'2024-06-25 07:14:31','demo94','DEMO94','94@ttdesignco.com','94@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECVdEihkGt835uOk5jeB+a9EHz6uhLDXtrWW+IgoE57vVYBGY0Z33vS2k+vQzhXf+A==','HSF5PCPRM7OBB55OE53LNCEKVHF5DTTX','d975be1f-e6f9-4873-bbca-daa76221184d',NULL,0,0,NULL,1,0),(96,'demo95',NULL,_binary '',0,'95',5,NULL,1,'2024-06-25 07:14:32',1,'2024-06-25 07:14:32','demo95','DEMO95','95@ttdesignco.com','95@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEGM/3EswGXtey993f8OhLUWUmxkzBy1gGEOYC9KcE2wiByorpvOhgat/cxyTZSCrYA==','3FOJMCDCDJRKXOLEGC53B2NA3A6UYPOV','d4ea5c48-40bd-4e89-a11b-5d977b27722c',NULL,0,0,NULL,1,0),(97,'demo96',NULL,_binary '',0,'96',5,NULL,1,'2024-06-25 07:14:34',1,'2024-06-25 07:14:34','demo96','DEMO96','96@ttdesignco.com','96@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMgN2yTFTy5IO/IilxgY2qP5PIB4MrcH3CosJfiapeIUgylY/iN+5pm0LlV+Y8hZ0Q==','FWERMF5H5P67LEFLCT54P2S26ERFBLKY','f3a7a634-1090-44aa-a855-9b94c0e07cf1',NULL,0,0,NULL,1,0),(98,'demo97',NULL,_binary '',0,'97',5,NULL,1,'2024-06-25 07:14:35',1,'2024-06-25 07:14:35','demo97','DEMO97','97@ttdesignco.com','97@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKj81On0Y2ZSOzBWYs8Tw1m4+zU5TTEDn/YgwPIEwxQHL5isjAjH2ndHxv9aoo4q5g==','YVMKN4ZJ3ESJIWOCO6GPCLJXJPMFAYYX','82c34255-165c-4eed-8200-229c8917e09f',NULL,0,0,NULL,1,0),(99,'demo98',NULL,_binary '',0,'98',5,NULL,1,'2024-06-25 07:14:36',1,'2024-06-25 07:14:36','demo98','DEMO98','98@ttdesignco.com','98@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECN3yBj8hSdKOr/zRj5GLJNOwx/OfK+jUjxKY8z1zERyZAyJFxWqE0vKIcRKD7Lj6g==','JO4KW5JNP7BPXH5QL5ST5UBXU7C64ZDB','5ca05085-07df-4494-abd8-3be4dad5a8cf',NULL,0,0,NULL,1,0),(100,'demo99',NULL,_binary '',0,'99',5,NULL,1,'2024-06-25 07:14:37',1,'2024-06-25 07:14:37','demo99','DEMO99','99@ttdesignco.com','99@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEBg0e2VxEWeTMPv8zcKUkQtpaxs1Hppf9DqazoqbO3Mwn7i3S3rp0IWDAy+vW6sAFA==','ZFGMZ4X3YWH2ICCJZ2HDJLJIYPHLPMP3','9874ad51-aefd-4e7a-90ee-fa0a967161de',NULL,0,0,NULL,1,0),(101,'demo100',NULL,_binary '',0,'100',5,NULL,1,'2024-06-28 07:25:58',1,'2024-06-28 07:25:58','demo100','DEMO100','100@ttdesignco.com','100@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPkc4nNe9mTdgnmRTNTjGKm/th4DRGCY0u20X7gn4NJQqPCAodfclyaTyfecqd/ICw==','54XTT2WBEW2DTS4ZCDSDKFSXXGAPQENY','23320fe6-ef99-479b-aa06-029140158f9e',NULL,0,0,NULL,1,0),(102,'demo101',NULL,_binary '',0,'101',5,NULL,1,'2024-06-28 07:25:59',1,'2024-06-28 07:25:59','demo101','DEMO101','101@ttdesignco.com','101@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPp5Is8UWmP0xc3RnAClDQ5OYb7KY2hKroFKA+91s60B4SJRw/bCQrACU6bYumI+2Q==','PUPPYF66ZNQUCKPT75SQQHQ3EF6RU7ZC','82a120f2-7680-484c-8287-510e3fbb0498',NULL,0,0,NULL,1,0),(103,'demo102',NULL,_binary '',0,'102',5,NULL,1,'2024-06-28 07:26:00',1,'2024-06-28 07:26:00','demo102','DEMO102','102@ttdesignco.com','102@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEH7o9a3hGpXTpmIgGSigm20FO/W4sNnAG3HlNspwWx4IWBrzqVRfiNs6HwTJrWGOVw==','4HKHMEZ4FFPNOSADWHRI32C22WK7KT4H','8743af1d-8f08-4dc4-b732-94053cada0d3',NULL,0,0,NULL,1,0),(104,'demo103',NULL,_binary '',0,'103',5,NULL,1,'2024-06-28 07:26:01',1,'2024-06-28 07:26:01','demo103','DEMO103','103@ttdesignco.com','103@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEGilN6wSqe3IaeqeJFmAxkORGRz30Z0T2HvQmnTtT7T/PJt4kYD6gHcny+UZrQWUJw==','HITZOPOI7FQGNKFWR2BNENISSEVWH2QD','c81264cd-4b96-42b3-b7bf-0c812c3dc13d',NULL,0,0,NULL,1,0),(105,'demo104',NULL,_binary '',0,'104',5,NULL,1,'2024-06-28 07:26:02',1,'2024-06-28 07:26:02','demo104','DEMO104','104@ttdesignco.com','104@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEIbixHDJF9xYVO49G6GE+h2MfcXt5bwOycoKWKDvXWyqhOz735VQ77MF30Ioi75MZg==','IFFSSFZWQ2LF5BBCUPNRX3G3GDEH2EH6','539a1e73-8371-4d6a-a108-93c4c5792a23',NULL,0,0,NULL,1,0),(106,'demo105',NULL,_binary '',0,'105',5,NULL,1,'2024-06-28 07:26:03',1,'2024-06-28 07:26:03','demo105','DEMO105','105@ttdesignco.com','105@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAED12PUpma5FiYJgmCdZyydygu+YMxiU3SH8RgqD4wqMF0NL3bd3/plIgHdtewAOCZg==','FE4F2STJ4C7HY6U52KVR65SYLDEGZHRE','6306ee3e-08a5-45f8-ae41-6c3556323fd8',NULL,0,0,NULL,1,0),(107,'demo106',NULL,_binary '',0,'106',5,NULL,1,'2024-06-28 07:26:04',1,'2024-06-28 07:26:04','demo106','DEMO106','106@ttdesignco.com','106@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFWDSK/qPpPmaSDksEa5kbqGiqol5iFnCq8vzv3Yhhe3kZFHh/W459UyhiWLaorwcw==','WTTK2DNP4ZSKUIDYFQI6GHHOHVCV3LFK','5bc85c28-5c81-4de7-8816-c89f2a6f82cd',NULL,0,0,NULL,1,0),(108,'demo107',NULL,_binary '',0,'107',5,NULL,1,'2024-06-28 07:26:04',1,'2024-06-28 07:26:04','demo107','DEMO107','107@ttdesignco.com','107@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJcG9czx9labJGi0UAuCN2WVYYlke2WvODwwHo3Q52OEENHRbCGnXKykJjs8fKYCzg==','YXYDPJILGBZ237ZZGA2GRZAFJOZ7ABG7','fed3921f-fe27-4e4b-9519-97a671cfd3d5',NULL,0,0,NULL,1,0),(109,'demo108',NULL,_binary '',0,'108',5,NULL,1,'2024-06-28 07:26:05',1,'2024-06-28 07:26:05','demo108','DEMO108','108@ttdesignco.com','108@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPPaUyNeKgQ9eTzqr7M4M8dAfCDMZeH5cb1L4+ceaJz66rUoQMI4DHBTUB1yDJLIiA==','3ZVR7TUE47422OYCP4IVYWX3I7TOQQ4Y','4e240bd5-0837-477e-bf6a-88880e249c90',NULL,0,0,NULL,1,0),(110,'demo109',NULL,_binary '',0,'109',5,NULL,1,'2024-06-28 07:26:06',1,'2024-06-28 07:26:06','demo109','DEMO109','109@ttdesignco.com','109@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEyaJqUsSq5FZQyTw2t+BwDgTLOWS2ek1/uRFYk4TjF2qj8iJF0BkUJxPK1rEsF3Gw==','SJPZKFF7IL5CQ5EHVTMVVF4A7CG3RXQH','5d9c7a7c-9f0c-4583-a1f7-918549f3b545',NULL,0,0,NULL,1,0),(111,'demo110',NULL,_binary '',0,'110',5,NULL,1,'2024-06-28 07:26:07',1,'2024-06-28 07:26:07','demo110','DEMO110','110@ttdesignco.com','110@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEIeMNw6FFONhHYWgozE7Q4npREROss+QQYfAkDHcryhyuGl/xdpJibO3KidlfRtUtA==','BKXAVGB22WNKBXTHSNU2ECUEGVSNHSYM','e6adb6eb-9bb1-4ac9-90cf-9c402397e08f',NULL,0,0,NULL,1,0),(112,'demo111',NULL,_binary '',0,'111',5,NULL,1,'2024-06-28 07:26:08',1,'2024-06-28 07:26:08','demo111','DEMO111','111@ttdesignco.com','111@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEB90aE3KKWiGVEODHQJxwRsn0W8Df5bbpG6kI62VoGFgNYhdKwWGbgCfyYIRafJhvA==','3KUISVSN2IFBYD6NMPJQDY7QQSP2TMTL','ba1b5fa5-5ba0-42f4-b5ee-6afc582b8d42',NULL,0,0,NULL,1,0),(113,'demo112',NULL,_binary '',0,'112',5,NULL,1,'2024-06-28 07:26:09',1,'2024-06-28 07:26:09','demo112','DEMO112','112@ttdesignco.com','112@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKOa61QtS4D0yRVULPHeI4U822vCeP+mGIcqRVqtuCHw2zV0oewhosFThZtzY37dbQ==','HD7UB33NEMEPKGEWJHTYTSHCXWJMBQE2','61bfea60-e6df-4ad8-96ec-277a681fd2ff',NULL,0,0,NULL,1,0),(114,'demo113',NULL,_binary '',0,'113',5,NULL,1,'2024-06-28 07:26:10',1,'2024-06-28 07:26:10','demo113','DEMO113','113@ttdesignco.com','113@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEO8Hw243k5dShaPdOXOWhTgQA8RrWpeCzG71FLJz/TD1YmyVSJmY0pBNo03p9JvPiQ==','IJZHC3GJIQ5VERNJNGHRQXUEJX4ZJLBU','05d7874f-00bc-414b-9a71-d3e911cf8ec1',NULL,0,0,NULL,1,0),(115,'demo114',NULL,_binary '',0,'114',5,NULL,1,'2024-06-28 07:26:11',1,'2024-06-28 07:26:11','demo114','DEMO114','114@ttdesignco.com','114@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEOyU3WxSYwLjfjgR6Gpru4b6o1koaTbVFeePsDwaLxaWGvNTZA6Trm0MfYBASH1g2w==','TFF42IF4MBL3SILYBG5T5NBWTAZAF2VZ','f460d85a-de8e-4e3e-9287-34121f968dad',NULL,0,0,NULL,1,0),(116,'demo115',NULL,_binary '',0,'115',5,NULL,1,'2024-06-28 07:26:12',1,'2024-06-28 07:26:12','demo115','DEMO115','115@ttdesignco.com','115@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEOrq8Cey26BxZg65Z9Bf1vQeXzF6Dl6VT93ec9gwUSI5BxlaJQiiEJpZbGYIgjilOQ==','MZVUV2ZK5HRMFVXLEWJONS33PBLEIJYW','6475cff7-ec91-4f46-b5f0-5202eff01ee3',NULL,0,0,NULL,1,0),(117,'demo116',NULL,_binary '',0,'116',5,NULL,1,'2024-06-28 07:26:13',1,'2024-06-28 07:26:13','demo116','DEMO116','116@ttdesignco.com','116@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEFLW5FUPj/EyxJ+Uqyy93XIqQgZHHiGSeXoHJgtXYQFq+0jkCrd9NZbkrkBwJ5SfUw==','IOOQZNFCK42KQJV56MIJDX36REFKC4FT','0cd49831-5d44-4866-9220-900517dbeb59',NULL,0,0,NULL,1,0),(118,'demo117',NULL,_binary '',0,'117',5,NULL,1,'2024-06-28 07:26:14',1,'2024-06-28 07:26:14','demo117','DEMO117','117@ttdesignco.com','117@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEIDyKr+qhWqQfFb/2kPlGEWXJSQolovDzbiytQsnXgNMQwEMJom/i8TkMdxc5639gw==','T6XCWWISU5ZTH3WAGA6IQMFZSS2BYFG6','295750ba-5446-47db-b576-8e4300f260e6',NULL,0,0,NULL,1,0),(119,'demo118',NULL,_binary '',0,'118',5,NULL,1,'2024-06-28 07:26:14',1,'2024-06-28 07:26:14','demo118','DEMO118','118@ttdesignco.com','118@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAENO/ErDC0kQ6P4AapbhWd5UaKNkEEa/NOMLZbnAGzS0Bg1L/Iz7UBEuByUKLD2ping==','MHAVNLNGP4RQUPAKA3FK7NGYN2XGOUKW','7d9d8e6d-14f7-4a72-bdde-021f7d5c0767',NULL,0,0,NULL,1,0),(120,'demo119',NULL,_binary '',0,'119',5,NULL,1,'2024-06-28 07:26:15',1,'2024-06-28 07:26:15','demo119','DEMO119','119@ttdesignco.com','119@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHRUGO/4f2CxkrlaTCKBjCCOlqPsd7fvF+Y5X/uQaTKV/7nooRtTFXVQCjsKz8RzJQ==','S5L2OSSHBKTYMXJ2NPEAA5G3ZRWMNSPB','3eb09cf9-cca4-4d34-9de8-cde07ecef194',NULL,0,0,NULL,1,0),(121,'demo120',NULL,_binary '',0,'120',5,NULL,1,'2024-06-28 07:26:16',1,'2024-06-28 07:26:16','demo120','DEMO120','120@ttdesignco.com','120@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHgkU1ozawQOmlYmpbemIFWQzVsJYmYfM+Qgso5WOpcxgfavlAMgXIPSsCMwWh+BGQ==','TDZCJYP5ZBQ62TGV5PQHDWOSU6PE6VIK','0ab0022e-3c82-419c-99cc-16a3c45b66bb',NULL,0,0,NULL,1,0),(122,'demo121',NULL,_binary '',0,'121',5,NULL,1,'2024-06-28 07:26:17',1,'2024-06-28 07:26:17','demo121','DEMO121','121@ttdesignco.com','121@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECx46kbWhwYNohFl76ZGBMeKZqHZUYJqtyfRZsJw/wGO0FkAfOC/xFLYYn16Quwk8w==','3RPRMGIY4FFRCJKGDKPULNVJYDI5EBML','2dcbbaa8-5ac9-4c5e-b27a-283aff6f0fe2',NULL,0,0,NULL,1,0),(123,'demo122',NULL,_binary '',0,'122',5,NULL,1,'2024-06-28 07:26:18',1,'2024-06-28 07:26:18','demo122','DEMO122','122@ttdesignco.com','122@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAENCKTZYw9rjPFtXx5AI2OsOlYc7vCQW23D0Kw41lb0uusOQC+yAx4NlG5vXBEBbd2A==','E5S3JYATSKJKSXU2U65XG7ABNK7LXEBR','c8272f04-46ec-4903-8054-f56f243fa694',NULL,0,0,NULL,1,0),(124,'demo123',NULL,_binary '',0,'123',5,NULL,1,'2024-06-28 07:26:19',1,'2024-06-28 07:26:19','demo123','DEMO123','123@ttdesignco.com','123@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJl7MMvc5yDPfSbgV5x+dVkcRMvc0xKKeCrdn/y2dD8MOWhHvcI1dlAAlGmlnvRK0g==','K3JBHQLLQ3EFUPJCLZHFZ3NZLAUJJJVJ','7eba40bf-f3b0-45ec-ad58-20518040ab8b',NULL,0,0,NULL,1,0),(125,'demo124',NULL,_binary '',0,'124',5,NULL,1,'2024-06-28 07:26:20',1,'2024-06-28 07:26:20','demo124','DEMO124','124@ttdesignco.com','124@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEJLaNkJReaLilL1SdK1O+lPUUitP0W6MhotRcJ6J6bbQDvy3ZZl8qNWg2bzvW6tfRw==','JWPJF66FIXOR3FYM2VEJOIUMLF4P47PS','bc823cb9-5abd-4fec-9333-64935f1589d1',NULL,0,0,NULL,1,0),(126,'demo125',NULL,_binary '',0,'125',5,NULL,1,'2024-06-28 07:26:21',1,'2024-06-28 07:26:21','demo125','DEMO125','125@ttdesignco.com','125@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEC7PKUNx//QVTSdyAqk49bz2A3PXXjHjG6WdqTMft58ktdZ0pqYj/dpceYuljriZxw==','NDESOA56F45SMKI7XJ4SGTGX2D4BWXAZ','5cd51a21-5f40-4458-bf98-d844af3bf14d',NULL,0,0,NULL,1,0),(127,'demo126',NULL,_binary '',0,'126',5,NULL,1,'2024-06-28 07:26:22',1,'2024-06-28 07:26:22','demo126','DEMO126','126@ttdesignco.com','126@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEMWbuWt18k8n+9r+9acl4bamoJnrErQEsJ1eAkdmHFBuwAHuRF1XqlypKgx0qd8ZQ==','4TZGP4Q2VCPNZVUNKE765KRXTBPMQGV3','ed14883f-050b-4697-ae9d-55e0829f128c',NULL,0,0,NULL,1,0),(128,'demo127',NULL,_binary '',0,'127',5,NULL,1,'2024-06-28 07:26:23',1,'2024-06-28 07:26:23','demo127','DEMO127','127@ttdesignco.com','127@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEETQ/vUMB7++OUfhdfmTOcBQqBFV7wwVPpy3QPPIen6230qazRt322V7bvb71utfUg==','KPJO2FUCO74JR2IXU52TOLCKC3DYGALO','1f3ec753-442b-4591-8ee4-eacce271e558',NULL,0,0,NULL,1,0),(129,'demo128',NULL,_binary '',0,'128',5,NULL,1,'2024-06-28 07:26:23',1,'2024-06-28 07:26:23','demo128','DEMO128','128@ttdesignco.com','128@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEAxbbgMxUPnOpJPGiJvYSWPMpt+iXEdkGVy/cl9flghf2kotnOa9ZmIl/3xZfq/QpA==','ULRKAOL4HQRK7KWI2ELM5CIJ5P6V6LJG','6b2e9ce5-d481-44a0-8227-a5f5010b9bed',NULL,0,0,NULL,1,0),(130,'demo129',NULL,_binary '',0,'129',5,NULL,1,'2024-06-28 07:26:24',1,'2024-06-28 07:26:24','demo129','DEMO129','129@ttdesignco.com','129@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPn3uH9A8rhXoKdVYwpQuRrY46l1ld7KsiJosdVmTCAnTVktn7+gUcv4mC0i8aUkPg==','DDRJUUWECEN3PR3XPRPHZLPAIYXDKMEM','8b183b66-e3b9-49f1-8762-f6c8013a8ebf',NULL,0,0,NULL,1,0),(131,'demo130',NULL,_binary '',0,'130',5,NULL,1,'2024-06-28 07:26:25',1,'2024-06-28 07:26:25','demo130','DEMO130','130@ttdesignco.com','130@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEMXN6sdZESIDjy+hf6TnvTGH8MCCfNV5hSCdx+DFjqmpbmFImopl6lF11eHU/7Ux7Q==','6TPRS3AUVMZXO7NEAMJKHMPJQD7VELQE','7094d53c-0ebd-401e-9b4c-87f8c5e439a5',NULL,0,0,NULL,1,0),(132,'demo131',NULL,_binary '',0,'131',5,NULL,1,'2024-06-28 07:26:26',1,'2024-06-28 07:26:26','demo131','DEMO131','131@ttdesignco.com','131@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEF7KWyud3y0pNPV/q/YBn6xpBlwZjLLO35gw0PK+lzMyglmh+BK50q5OktM7sw7YBQ==','ZQME373KSGO35X4JWN74QGZRSFPYTPB5','0bdb6763-b743-44eb-b114-7252bb67a708',NULL,0,0,NULL,1,0),(133,'demo132',NULL,_binary '',0,'132',5,NULL,1,'2024-06-28 07:26:27',1,'2024-06-28 07:26:27','demo132','DEMO132','132@ttdesignco.com','132@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEHjymMqznSt3iLd2Bf9V6Rti6D2D5AV/nd0Cuc/PduQfVjMLJPCh1JK2tLWy1svW6w==','IQLNBVTFR6IAEHX5QEUBTOXHTNV557L5','69fd5fd3-9cae-46a9-8029-00e307a32c37',NULL,0,0,NULL,1,0),(134,'demo133',NULL,_binary '',0,'133',5,NULL,1,'2024-06-28 07:26:28',1,'2024-06-28 07:26:28','demo133','DEMO133','133@ttdesignco.com','133@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEO4yhagayHdLTJg4N5XULMMwisWBNgI3lLH6AqjKkBcOfWbzP/E+dUUIKk6Tzuo/MQ==','BPS5NOBK7XLTR74HUA42WTCPSNPBVOYY','6e48d86e-b071-4151-8630-4dc6ebd3a798',NULL,0,0,NULL,1,0),(135,'demo134',NULL,_binary '',0,'134',5,NULL,1,'2024-06-28 07:26:29',1,'2024-06-28 07:26:29','demo134','DEMO134','134@ttdesignco.com','134@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEH6XQ8fOR6tVNJyPM+5C1cKROwkXy1Gg9PjYz5ikBxTXehRq0sjAozpULmkRbs540Q==','I4ZV2MSDQJBYZ7YQWS3ZZQ6L6ZZ7CHX6','2792068c-31cc-4c35-857e-21dadeeffac9',NULL,0,0,NULL,1,0),(136,'demo135',NULL,_binary '',0,'135',5,NULL,1,'2024-06-28 07:26:30',1,'2024-06-28 07:26:30','demo135','DEMO135','135@ttdesignco.com','135@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEPZi54nqhZVNT+EIUEBaDh6gdaQD/nMG+oziM4X76UKQeNfKNnIly1EQfD1ITehgKQ==','BUGLKUGI4UP4EH6FULW2Z7MBYS3OTO7D','b6b6de51-681b-41eb-822f-c48f17c3721d',NULL,0,0,NULL,1,0),(137,'demo136',NULL,_binary '',0,'136',5,NULL,1,'2024-06-28 07:26:31',1,'2024-06-28 07:26:31','demo136','DEMO136','136@ttdesignco.com','136@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEEtvMRGL74vzuMBVjeED7DETbGenLtP4diwmQWTnILUZGNeRVDNeuC0znne9aylBqw==','IHFIWEW23TDBDW5754NOL65HXNUQC44S','ff5e32f0-0069-44bc-ac2c-548f2a34dcd2',NULL,0,0,NULL,1,0),(138,'demo137',NULL,_binary '',0,'137',5,NULL,1,'2024-06-28 07:26:32',1,'2024-06-28 07:26:32','demo137','DEMO137','137@ttdesignco.com','137@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKu0s2SfpgYQgWUF2JOLswlYOc+d0qnOCScNv87RUhaqS/X2X6dwRwt65gvKWG26sQ==','KF67AY75GGIB2KLUREZOU4IKWEOHD6AH','89fc0be9-4b8f-4785-90c6-672fd60c97b4',NULL,0,0,NULL,1,0),(139,'demo138',NULL,_binary '',0,'138',5,NULL,1,'2024-06-28 07:26:32',1,'2024-06-28 07:26:32','demo138','DEMO138','138@ttdesignco.com','138@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEDPagxwaH3BD1gaixbvZp/dU5iurd/dbO4zd4kBfyVoKR0zn20k3CK00KAzxfcyAnQ==','XVYQQTCFE52GIOWVMKXYBLEB4I3PBV47','f1445e27-d1a6-4821-9141-75b4a85b2858',NULL,0,0,NULL,1,0),(140,'demo139',NULL,_binary '',0,'139',5,NULL,1,'2024-06-28 07:26:33',1,'2024-06-28 07:26:33','demo139','DEMO139','139@ttdesignco.com','139@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEKkIunH/59C3dkmqXQVKQ+89eRbbO/8ANjw9GBa1ZrEMTS5owTAAwh6gg8QFgtJJ9A==','6ZFSO35C3JU7UPEB7UEK4PQGXQHDQQLG','834c9df6-3bb5-4b34-9874-acd31ab61904',NULL,0,0,NULL,1,0),(141,'demo140',NULL,_binary '',0,'140',5,NULL,1,'2024-06-28 07:26:34',1,'2024-06-28 07:26:34','demo140','DEMO140','140@ttdesignco.com','140@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEK5kPlQG9AYWQ6Wsuz8qsb0YL26eiDNHUxAOQxNR9nmawgywE2MsDY/5ENYqgq/2wA==','TOZROD3SMVBTYOJZXSSDWCYHF2VVFJMJ','162f5ecb-46c7-49d6-8187-4816c238078b',NULL,0,0,NULL,1,0),(142,'demo141',NULL,_binary '',0,'141',5,NULL,1,'2024-06-28 07:26:35',1,'2024-06-28 07:26:35','demo141','DEMO141','141@ttdesignco.com','141@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEDRqS48IVxSwxZiFrLhigf64pPHm5qikaeDTAJxPIttxG91yNeeAqnNa0z+RzT0jag==','PCSTGUJQ5S5E4ZDKCJ27ZUJRTJQVJNWY','54a07742-13df-4e5a-8296-af50a1496d3e',NULL,0,0,NULL,1,0),(143,'demo142',NULL,_binary '',0,'142',5,NULL,1,'2024-06-28 07:26:36',1,'2024-06-28 07:26:36','demo142','DEMO142','142@ttdesignco.com','142@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAECDI4MBSo2OBNtsI6XuRPWfn25XjxlsBLbjgaIHyRkTxIGtmTPOzncUua7A9OQ/6LQ==','G3CLUJKORHSPHOQUKUWYI4M6G7IZHV6W','44749cbf-5fab-44d5-8c14-5185cc940a97',NULL,0,0,NULL,1,0),(144,'demo143',NULL,_binary '',0,'143',5,NULL,1,'2024-06-28 07:26:37',1,'2024-06-28 07:26:37','demo143','DEMO143','143@ttdesignco.com','143@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAELHfU09h1kP2Ss55Yp3vsGuMRWTDdQKTEXgBnD/Wk2FHP9UmBHiIzqnLkb5PHf3ClQ==','YEAZSW743ZCMYRLL4CBMD74ZD6U6SKBK','bf36fc6f-5d28-4181-995a-25c1c2435f3a',NULL,0,0,NULL,1,0),(145,'demo144',NULL,_binary '',0,'144',5,NULL,1,'2024-06-28 07:26:38',1,'2024-06-28 07:26:38','demo144','DEMO144','144@ttdesignco.com','144@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEIRtQpmVhW1brUMT/8FB8Hf+9WSAbJ0useq6fD4/Hvx0Kh+avbF+5eNVKV7a3uKZSA==','NWDADOFD23KDILH4AHKLF2YFIL23HTYU','90639116-4f9f-49cf-b92e-1b1a134e48b8',NULL,0,0,NULL,1,0),(146,'demo145',NULL,_binary '',0,'145',5,NULL,1,'2024-06-28 07:26:39',1,'2024-06-28 07:26:39','demo145','DEMO145','145@ttdesignco.com','145@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEOpIzEC7qSWfIpBU/HgSDjDNR0e/AMbgtNchu+4zKuKrWTqEDQsWiyVBLUbI+3cxTA==','JD6PFK2FITPPAH6MVIKJKFHAYHYAFMU5','f05224f5-ab02-4d9d-b20a-62b4e40e59d4',NULL,0,0,NULL,1,0),(147,'demo146',NULL,_binary '',0,'146',5,NULL,1,'2024-06-28 07:26:40',1,'2024-06-28 07:26:40','demo146','DEMO146','146@ttdesignco.com','146@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEM2NqCc02IUpwcjB9Mx2xzOyFt7fJujVGW8K4LYV/zZTO0WWzbrh245evK2mujv2Ow==','LAM5J7F2CAQWM6SPQ3G3GWSUZROTSND4','daca696d-8523-4722-b1fe-16fe93973dc4',NULL,0,0,NULL,1,0),(148,'demo147',NULL,_binary '',0,'147',5,NULL,1,'2024-06-28 07:26:40',1,'2024-06-28 07:26:40','demo147','DEMO147','147@ttdesignco.com','147@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAENnSwceezszbx9nXhik06nDvkG30ZdrJ5mQOCErK4npdMZzJ0QSbQstsEqCxYxJ5ew==','5KWLQWWXZ3TDX4WXURW3EE5HAWEG7OCK','52103528-a58a-43ac-9820-78627c3aeda3',NULL,0,0,NULL,1,0),(149,'demo148',NULL,_binary '',0,'148',5,NULL,1,'2024-06-28 07:26:41',1,'2024-06-28 07:26:41','demo148','DEMO148','148@ttdesignco.com','148@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEO8aWUu/oAZrAEBxMlVGeKDOgH/QgjfkQBzwxjbbr1+7wfFccNdP8Pu2UkwT0/9hgA==','JVGTAZGWU6JJRS5AVNV5X2LKNE6TEOEG','a85f4515-9b8e-4e78-8b15-c9ff9c42c952',NULL,0,0,NULL,1,0),(150,'demo149',NULL,_binary '',0,'149',5,NULL,1,'2024-06-28 07:26:42',1,'2024-06-28 07:26:42','demo149','DEMO149','149@ttdesignco.com','149@TTDESIGNCO.COM',0,'AQAAAAEAACcQAAAAEI6JRknh8WQEVVJIc4CpP2Av+tDEm/GAGvWEPMIUzZ82JEiJV58ZB6Afdmu73Bp4Ww==','LSNBZC6EQWRF25WR52BOJB4RHUKAPS77','29e91428-360e-4f5d-b810-56a8b4d1b5b8',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `User` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `UserClaims`
--

LOCK TABLES `UserClaims` WRITE;
/*!40000 ALTER TABLE `UserClaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserClaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `UserInfo`
--

LOCK TABLES `UserInfo` WRITE;
/*!40000 ALTER TABLE `UserInfo` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserInfo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `UserLogins`
--

LOCK TABLES `UserLogins` WRITE;
/*!40000 ALTER TABLE `UserLogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserLogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `UserRoles`
--

LOCK TABLES `UserRoles` WRITE;
/*!40000 ALTER TABLE `UserRoles` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserRoles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `UserSetting`
--

LOCK TABLES `UserSetting` WRITE;
/*!40000 ALTER TABLE `UserSetting` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserSetting` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `UserTokens`
--

LOCK TABLES `UserTokens` WRITE;
/*!40000 ALTER TABLE `UserTokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserTokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `WfhRequest`
--

LOCK TABLES `WfhRequest` WRITE;
/*!40000 ALTER TABLE `WfhRequest` DISABLE KEYS */;
/*!40000 ALTER TABLE `WfhRequest` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20240624061556_Initial','6.0.2');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-06-28 14:29:20
