﻿using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("FromQueueMailSender.UnitTests")]
namespace FromQueueMailSender.Services.Mail.Formats
{
    internal class ActivateAccountContent
    {
        private static readonly string contentFirstPart = "<!doctype html>\r\n<html ⚡4email data-css-strict>\r\n<head>\r\n    <meta charset=\"utf-8\">\r\n    <style amp4email-boilerplate>\r\n        body {\r\n            visibility: hidden\r\n        }\r\n    </style>\r\n    <script async src=\"https://cdn.ampproject.org/v0.js\"></script>\r\n    <style amp-custom>\r\n        .es-desk-hidden {\r\n            display: none;\r\n            float: left;\r\n            overflow: hidden;\r\n            width: 0;\r\n            max-height: 0;\r\n            line-height: 0;\r\n        }\r\n\r\n        .es-button-border:hover a.es-button, .es-button-border:hover button.es-button {\r\n            background: #56D66B;\r\n        }\r\n\r\n        .es-button-border:hover {\r\n            border-color: #42D159 #42D159 #42D159 #42D159;\r\n            background: #56D66B;\r\n        }\r\n\r\n        body {\r\n            width: 100%;\r\n            font-family: arial, \"helvetica neue\", helvetica, sans-serif;\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse;\r\n            border-spacing: 0px;\r\n        }\r\n\r\n            table td, body, .es-wrapper {\r\n                padding: 0;\r\n                Margin: 0;\r\n            }\r\n\r\n        .es-content, .es-header, .es-footer {\r\n            table-layout: fixed;\r\n            width: 100%;\r\n        }\r\n\r\n        p, hr {\r\n            Margin: 0;\r\n        }\r\n\r\n        h1, h2, h3, h4, h5 {\r\n            Margin: 0;\r\n            line-height: 120%;\r\n            font-family: arial, \"helvetica neue\", helvetica, sans-serif;\r\n        }\r\n\r\n        .es-left {\r\n            float: left;\r\n        }\r\n\r\n        .es-right {\r\n            float: right;\r\n        }\r\n\r\n        .es-p5 {\r\n            padding: 5px;\r\n        }\r\n\r\n        .es-p5t {\r\n            padding-top: 5px;\r\n        }\r\n\r\n        .es-p5b {\r\n            padding-bottom: 5px;\r\n        }\r\n\r\n        .es-p5l {\r\n            padding-left: 5px;\r\n        }\r\n\r\n        .es-p5r {\r\n            padding-right: 5px;\r\n        }\r\n\r\n        .es-p10 {\r\n            padding: 10px;\r\n        }\r\n\r\n        .es-p10t {\r\n            padding-top: 10px;\r\n        }\r\n\r\n        .es-p10b {\r\n            padding-bottom: 10px;\r\n        }\r\n\r\n        .es-p10l {\r\n            padding-left: 10px;\r\n        }\r\n\r\n        .es-p10r {\r\n            padding-right: 10px;\r\n        }\r\n\r\n        .es-p15 {\r\n            padding: 15px;\r\n        }\r\n\r\n        .es-p15t {\r\n            padding-top: 15px;\r\n        }\r\n\r\n        .es-p15b {\r\n            padding-bottom: 15px;\r\n        }\r\n\r\n        .es-p15l {\r\n            padding-left: 15px;\r\n        }\r\n\r\n        .es-p15r {\r\n            padding-right: 15px;\r\n        }\r\n\r\n        .es-p20 {\r\n            padding: 20px;\r\n        }\r\n\r\n        .es-p20t {\r\n            padding-top: 20px;\r\n        }\r\n\r\n        .es-p20b {\r\n            padding-bottom: 20px;\r\n        }\r\n\r\n        .es-p20l {\r\n            padding-left: 20px;\r\n        }\r\n\r\n        .es-p20r {\r\n            padding-right: 20px;\r\n        }\r\n\r\n        .es-p25 {\r\n            padding: 25px;\r\n        }\r\n\r\n        .es-p25t {\r\n            padding-top: 25px;\r\n        }\r\n\r\n        .es-p25b {\r\n            padding-bottom: 25px;\r\n        }\r\n\r\n        .es-p25l {\r\n            padding-left: 25px;\r\n        }\r\n\r\n        .es-p25r {\r\n            padding-right: 25px;\r\n        }\r\n\r\n        .es-p30 {\r\n            padding: 30px;\r\n        }\r\n\r\n        .es-p30t {\r\n            padding-top: 30px;\r\n        }\r\n\r\n        .es-p30b {\r\n            padding-bottom: 30px;\r\n        }\r\n\r\n        .es-p30l {\r\n            padding-left: 30px;\r\n        }\r\n\r\n        .es-p30r {\r\n            padding-right: 30px;\r\n        }\r\n\r\n        .es-p35 {\r\n            padding: 35px;\r\n        }\r\n\r\n        .es-p35t {\r\n            padding-top: 35px;\r\n        }\r\n\r\n        .es-p35b {\r\n            padding-bottom: 35px;\r\n        }\r\n\r\n        .es-p35l {\r\n            padding-left: 35px;\r\n        }\r\n\r\n        .es-p35r {\r\n            padding-right: 35px;\r\n        }\r\n\r\n        .es-p40 {\r\n            padding: 40px;\r\n        }\r\n\r\n        .es-p40t {\r\n            padding-top: 40px;\r\n        }\r\n\r\n        .es-p40b {\r\n            padding-bottom: 40px;\r\n        }\r\n\r\n        .es-p40l {\r\n            padding-left: 40px;\r\n        }\r\n\r\n        .es-p40r {\r\n            padding-right: 40px;\r\n        }\r\n\r\n        .es-menu td {\r\n            border: 0;\r\n        }\r\n\r\n        s {\r\n            text-decoration: line-through;\r\n        }\r\n\r\n        p, ul li, ol li {\r\n            font-family: arial, \"helvetica neue\", helvetica, sans-serif;\r\n            line-height: 150%;\r\n        }\r\n\r\n        ul li, ol li {\r\n            Margin-bottom: 15px;\r\n            margin-left: 0;\r\n        }\r\n\r\n        a {\r\n            text-decoration: underline;\r\n        }\r\n\r\n        .es-menu td a {\r\n            text-decoration: none;\r\n            display: block;\r\n            font-family: arial, \"helvetica neue\", helvetica, sans-serif;\r\n        }\r\n\r\n        .es-menu amp-img, .es-button amp-img {\r\n            vertical-align: middle;\r\n        }\r\n\r\n        .es-wrapper {\r\n            width: 100%;\r\n            height: 100%;\r\n        }\r\n\r\n        .es-wrapper-color, .es-wrapper {\r\n            background-color: #F6F6F6;\r\n        }\r\n\r\n        .es-header {\r\n            background-color: transparent;\r\n        }\r\n\r\n        .es-header-body {\r\n            background-color: #FFFFFF;\r\n        }\r\n\r\n            .es-header-body p, .es-header-body ul li, .es-header-body ol li {\r\n                color: #333333;\r\n                font-size: 14px;\r\n            }\r\n\r\n            .es-header-body a {\r\n                color: #2CB543;\r\n                font-size: 14px;\r\n            }\r\n\r\n        .es-content-body {\r\n            background-color: #FFFFFF;\r\n        }\r\n\r\n            .es-content-body p, .es-content-body ul li, .es-content-body ol li {\r\n                color: #333333;\r\n                font-size: 14px;\r\n            }\r\n\r\n            .es-content-body a {\r\n                color: #2CB543;\r\n                font-size: 14px;\r\n            }\r\n\r\n        .es-footer {\r\n            background-color: transparent;\r\n        }\r\n\r\n        .es-footer-body {\r\n            background-color: #FFFFFF;\r\n        }\r\n\r\n            .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li {\r\n                color: #333333;\r\n                font-size: 14px;\r\n            }\r\n\r\n            .es-footer-body a {\r\n                color: #FFFFFF;\r\n                font-size: 14px;\r\n            }\r\n\r\n        .es-infoblock, .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li {\r\n            line-height: 120%;\r\n            font-size: 12px;\r\n            color: #CCCCCC;\r\n        }\r\n\r\n            .es-infoblock a {\r\n                font-size: 12px;\r\n                color: #CCCCCC;\r\n            }\r\n\r\n        h1 {\r\n            font-size: 30px;\r\n            font-style: normal;\r\n            font-weight: normal;\r\n            color: #333333;\r\n        }\r\n\r\n        h2 {\r\n            font-size: 24px;\r\n            font-style: normal;\r\n            font-weight: normal;\r\n            color: #333333;\r\n        }\r\n\r\n        h3 {\r\n            font-size: 20px;\r\n            font-style: normal;\r\n            font-weight: normal;\r\n            color: #333333;\r\n        }\r\n\r\n        .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a {\r\n            font-size: 30px;\r\n        }\r\n\r\n        .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a {\r\n            font-size: 24px;\r\n        }\r\n\r\n        .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a {\r\n            font-size: 20px;\r\n        }\r\n\r\n        a.es-button, button.es-button {\r\n            display: inline-block;\r\n            background: #31CB4B;\r\n            border-radius: 30px;\r\n            font-size: 18px;\r\n            font-family: arial, \"helvetica neue\", helvetica, sans-serif;\r\n            font-weight: normal;\r\n            font-style: normal;\r\n            line-height: 120%;\r\n            color: #FFFFFF;\r\n            text-decoration: none;\r\n            width: auto;\r\n            text-align: center;\r\n            padding: 10px 20px 10px 20px;\r\n        }\r\n\r\n        .es-button-border {\r\n            border-style: solid solid solid solid;\r\n            border-color: #2CB543 #2CB543 #2CB543 #2CB543;\r\n            background: #31CB4B;\r\n            border-width: 0px 0px 2px 0px;\r\n            display: inline-block;\r\n            border-radius: 30px;\r\n            width: auto;\r\n        }\r\n\r\n        body {\r\n            font-family: arial, \"helvetica neue\", helvetica, sans-serif;\r\n        }\r\n\r\n        @media only screen and (max-width:600px) {\r\n            p, ul li, ol li, a {\r\n                line-height: 150%\r\n            }\r\n\r\n            h1, h2, h3, h1 a, h2 a, h3 a {\r\n                line-height: 120%\r\n            }\r\n\r\n            h1 {\r\n                font-size: 30px;\r\n                text-align: left\r\n            }\r\n\r\n            h2 {\r\n                font-size: 24px;\r\n                text-align: left\r\n            }\r\n\r\n            h3 {\r\n                font-size: 20px;\r\n                text-align: left\r\n            }\r\n\r\n            .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a {\r\n                font-size: 30px;\r\n                text-align: left\r\n            }\r\n\r\n            .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a {\r\n                font-size: 24px;\r\n                text-align: left\r\n            }\r\n\r\n            .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a {\r\n                font-size: 20px;\r\n                text-align: left\r\n            }\r\n\r\n            .es-menu td a {\r\n                font-size: 14px\r\n            }\r\n\r\n            .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a {\r\n                font-size: 14px\r\n            }\r\n\r\n            .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a {\r\n                font-size: 14px\r\n            }\r\n\r\n            .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a {\r\n                font-size: 14px\r\n            }\r\n\r\n            .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a {\r\n                font-size: 12px\r\n            }\r\n\r\n            *[class=\"gmail-fix\"] {\r\n                display: none\r\n            }\r\n\r\n            .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 {\r\n                text-align: center\r\n            }\r\n\r\n            .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 {\r\n                text-align: right\r\n            }\r\n\r\n            .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 {\r\n                text-align: left\r\n            }\r\n\r\n            .es-m-txt-r amp-img {\r\n                float: right\r\n            }\r\n\r\n            .es-m-txt-c amp-img {\r\n                margin: 0 auto\r\n            }\r\n\r\n            .es-m-txt-l amp-img {\r\n                float: left\r\n            }\r\n\r\n            .es-button-border {\r\n                display: inline-block\r\n            }\r\n\r\n            a.es-button, button.es-button {\r\n                font-size: 18px;\r\n                display: inline-block\r\n            }\r\n\r\n            .es-adaptive table, .es-left, .es-right {\r\n                width: 100%\r\n            }\r\n\r\n            .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header {\r\n                width: 100%;\r\n                max-width: 600px\r\n            }\r\n\r\n            .es-adapt-td {\r\n                display: block;\r\n                width: 100%\r\n            }\r\n\r\n            .adapt-img {\r\n                width: 100%;\r\n                height: auto\r\n            }\r\n\r\n            td.es-m-p0 {\r\n                padding: 0px\r\n            }\r\n\r\n            td.es-m-p0r {\r\n                padding-right: 0px\r\n            }\r\n\r\n            td.es-m-p0l {\r\n                padding-left: 0px\r\n            }\r\n\r\n            td.es-m-p0t {\r\n                padding-top: 0px\r\n            }\r\n\r\n            td.es-m-p0b {\r\n                padding-bottom: 0\r\n            }\r\n\r\n            td.es-m-p20b {\r\n                padding-bottom: 20px\r\n            }\r\n\r\n            .es-mobile-hidden, .es-hidden {\r\n                display: none\r\n            }\r\n\r\n            tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden {\r\n                width: auto;\r\n                overflow: visible;\r\n                float: none;\r\n                max-height: inherit;\r\n                line-height: inherit\r\n            }\r\n\r\n            tr.es-desk-hidden {\r\n                display: table-row\r\n            }\r\n\r\n            table.es-desk-hidden {\r\n                display: table\r\n            }\r\n\r\n            td.es-desk-menu-hidden {\r\n                display: table-cell\r\n            }\r\n\r\n            .es-menu td {\r\n                width: 1%\r\n            }\r\n\r\n            table.es-table-not-adapt, .esd-block-html table {\r\n                width: auto\r\n            }\r\n\r\n            table.es-social {\r\n                display: inline-block\r\n            }\r\n\r\n                table.es-social td {\r\n                    display: inline-block\r\n                }\r\n\r\n            .es-desk-hidden {\r\n                display: table-row;\r\n                width: auto;\r\n                overflow: visible;\r\n                max-height: inherit\r\n            }\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"es-wrapper-color\">\r\n        <!--[if gte mso 9]><v:background xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"t\"> <v:fill type=\"tile\" color=\"#f6f6f6\"></v:fill> </v:background><![endif]--><table class=\"es-wrapper\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n            <tr>\r\n                <td valign=\"top\">\r\n                    <table class=\"es-header\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">\r\n                        <tr>\r\n                            <td align=\"center\"><table class=\"es-header-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\"><tr><td class=\"es-p20t es-p20r es-p20l\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"560\" align=\"center\" valign=\"top\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tr><td align=\"center\"><p style=\"font-size: 20px;line-height: 40px;font-family: lato, 'helvetica neue', helvetica, arial, sans-serif\">Potwierdzenie konta w serwisie JTM</p></td></tr></table></td></tr></table></td></tr></table></td>\r\n                        </tr>\r\n                    </table><table class=\"es-content\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">\r\n                        <tr>\r\n                            <td align=\"center\">\r\n                                <table class=\"es-content-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\">\r\n                                    <tr>\r\n                                        <td class=\"es-p20t es-p20r es-p20l\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"560\" align=\"center\" valign=\"top\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tr><td align=\"center\" class=\"es-p20\" style=\"font-size:0\"><table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr><td style=\"border-bottom: 1px solid #cccccc;background: unset;height:1px;width:100%;margin:0px 0px 0px 0px\"></td></tr></table></td></tr></table></td></tr></table></td>\r\n                                    </tr>\r\n                                    <tr>\r\n                                        <td class=\"es-p20t es-p20r es-p20l\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"560\" align=\"center\" valign=\"top\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tr><td align=\"center\"><p style=\"font-size: 16px;font-family: lato, 'helvetica neue', helvetica, arial, sans-serif\">Cześć ";
        private static readonly string contentSecondPart = ",</p><p style=\"font-size: 16px;font-family: lato, 'helvetica neue', helvetica, arial, sans-serif\">Aby aktywować konto w serwisie JTM kliknij poniższy przycisk.</p></td></tr></table></td></tr></table></td>\r\n                                    </tr>\r\n                                    <tr>\r\n                                        <td class=\"es-p20t es-p20r es-p20l\" align=\"left\">\r\n                                            <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\">\r\n                                                <tr>\r\n                                                    <td width=\"560\" valign=\"top\" align=\"center\">\r\n                                                        <table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\">\r\n                                                            <tr>\r\n                                                                <td align=\"center\"> <!--[if mso]><a href=\"";
        private static readonly string contentThirdPart = "\" target=\"_blank\" hidden> <v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" esdevVmlButton href=\"";
        private static readonly string contentFourthPart = "\" style=\"height:39px; v-text-anchor:middle; width:193px\" arcsize=\"50%\" strokecolor=\"#2cb543\" strokeweight=\"2px\" fillcolor=\"#31cb4b\"> <w:anchorlock></w:anchorlock> <center style='color:#ffffff; font-family:lato, \"helvetica neue\", helvetica, arial, sans-serif; font-size:14px; font-weight:400; line-height:14px; mso-text-raise:1px'>Potwierdź konto</center> </v:roundrect></a><![endif]--> <!--[if !mso]><!-- --><span class=\"msohide es-button-border\"><a href=\"";
        private static readonly string contentFifthPart = "\" class=\"es-button\" target=\"_blank\" style=\"font-family: lato, &quot;helvetica neue&quot;, helvetica, arial, sans-serif\">Potwierdź konto</a></span> <!--<![endif]--></td>\r\n                                                            </tr>\r\n                                                        </table>\r\n                                                    </td>\r\n                                                </tr>\r\n                                            </table>\r\n                                        </td>\r\n                                    </tr>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                    </table><table class=\"es-footer\" cellspacing=\"0\" cellpadding=\"0\" align=\"center\">\r\n                        <tr>\r\n                            <td align=\"center\">\r\n                                <table class=\"es-footer-body\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\" bgcolor=\"#ffffff\" align=\"center\">\r\n                                    <tr>\r\n                                        <td class=\"es-p20t es-p20r es-p20l\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"560\" align=\"center\" valign=\"top\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tr><td align=\"center\" class=\"es-p20\" style=\"font-size:0\"><table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr><td style=\"border-bottom: 1px solid #cccccc;background: unset;height:1px;width:100%;margin:0px 0px 0px 0px\"></td></tr></table></td></tr></table></td></tr></table></td>\r\n                                    </tr>\r\n                                    <tr>\r\n                                        <td class=\"es-p20t es-p20r es-p20l\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"560\" align=\"center\" valign=\"top\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tr><td align=\"center\"><p style=\"line-height: 30px;font-size: 15px;color: #999999;font-family: lato, 'helvetica neue', helvetica, arial, sans-serif\">Wiadomość wygenerowana automatycznie prosimy na nią nie odpowiadać.</p></td></tr></table></td></tr></table></td>\r\n                                    </tr>\r\n                                    <tr>\r\n                                        <td class=\"es-p20t es-p20r es-p20l\" align=\"left\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td width=\"560\" align=\"center\" valign=\"top\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" role=\"presentation\"><tr><td align=\"center\" style=\"font-size: 0px\"><amp-img class=\"adapt-img\" src=\"https://mqhldf.stripocdn.email/content/guids/CABINET_5d6c8caf73e06ff9196c04579e43808a93ed2e17a778c03533afd5362d00edf2/images/logonobackground.png\" alt style=\"display: block\" width=\"250\" height=\"277\" layout=\"responsive\"></amp-img></td></tr></table></td></tr></table></td>\r\n                                    </tr>\r\n                                    <tr><td class=\"es-p20t es-p20b es-p20r es-p20l\" align=\"left\"> <!--[if mso]><table width=\"560\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"270\" valign=\"top\"><![endif]--><table class=\"es-left\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\"><tr><td class=\"es-m-p20b\" width=\"270\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\"><tr><td align=\"center\" style=\"display: none\"></td></tr></table></td></tr></table> <!--[if mso]></td><td width=\"20\"></td><td width=\"270\" valign=\"top\"><![endif]--><table class=\"es-right\" cellspacing=\"0\" cellpadding=\"0\" align=\"right\"><tr><td width=\"270\" align=\"left\"><table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" role=\"presentation\"><tr><td align=\"right\"><p style=\"line-height: 30px;font-size: 15px;color: #999999\">Jakub Michalak</p></td></tr></table></td></tr></table> <!--[if mso]></td></tr></table><![endif]--></td></tr>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                    </table>\r\n                </td>\r\n            </tr>\r\n        </table>\r\n    </div>\r\n</body>\r\n</html>";
        private static StringBuilder? contentBuilder;

        public static string GetContent(string name, string url)
        {
            contentBuilder = new StringBuilder();
            contentBuilder.Append(contentFirstPart);
            contentBuilder.Append(name);
            contentBuilder.Append(contentSecondPart);
            contentBuilder.Append(url);
            contentBuilder.Append(contentThirdPart);
            contentBuilder.Append(url);
            contentBuilder.Append(contentFourthPart);
            contentBuilder.Append(url);
            contentBuilder.Append(contentFifthPart);
            return contentBuilder.ToString();
        }
    }
}