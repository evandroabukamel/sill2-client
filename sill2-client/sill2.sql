CREATE DATABASE  IF NOT EXISTS `sill2` /*!40100 DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci */;
USE `sill2`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 10.15.0.253    Database: sill2
-- ------------------------------------------------------
-- Server version	5.5.49-0+deb7u1

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `freeusers`
--

DROP TABLE IF EXISTS `freeusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `freeusers` (
  `login` varchar(16) NOT NULL,
  PRIMARY KEY (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Tabela de usuarios com sessoes ilimitadas.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `history`
--

DROP TABLE IF EXISTS `history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `history` (
  `user` varchar(32) NOT NULL,
  `computer` varchar(32) NOT NULL,
  `logon` datetime NOT NULL,
  `logoff` datetime NOT NULL,
  `chkrow` varchar(32) NOT NULL,
  PRIMARY KEY (`chkrow`),
  KEY `user` (`user`) USING BTREE,
  KEY `computer` (`computer`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Historico de usuarios logados anteriormente.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `logged`
--

DROP TABLE IF EXISTS `logged`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `logged` (
  `user` varchar(32) NOT NULL,
  `computer` varchar(16) NOT NULL,
  `logon` datetime DEFAULT NULL,
  `lognow` datetime DEFAULT NULL,
  PRIMARY KEY (`computer`),
  KEY `user` (`user`),
  KEY `computer` (`computer`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-12-11 21:17:52
