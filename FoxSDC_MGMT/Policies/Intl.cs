using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FoxSDC_Common;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Threading;
using System.Globalization;

namespace FoxSDC_MGMT.Policies
{
    public partial class ctlIntl : UserControl, PolicyElementInterface
    {
        PolicyObject Pol;
        InternationalPolicy IntlSettings;

        CheckState C(bool? b)
        {
            switch (b)
            {
                case true:
                    return (CheckState.Checked);
                case false:
                    return (CheckState.Unchecked);
                default:
                    return (CheckState.Indeterminate);
            }
        }

        bool? C(CheckState b)
        {
            switch (b)
            {
                case CheckState.Checked:
                    return (true);
                case CheckState.Unchecked:
                    return (false);
                default:
                    return (null);
            }
        }

        class HomeLocation
        {
            public HomeLocation(int ID, string Name)
            {
                this.ID = ID;
                this.Name = Name;
            }

            public HomeLocation()
            {

            }

            public override string ToString()
            {
                return (Name);
            }

            public int ID;
            public string Name;
        }

        #region Home Locations

        List<HomeLocation> hloc = new List<HomeLocation>()
        {
            new HomeLocation(2,"Antigua and Barbuda"),
            new HomeLocation(3,"Afghanistan"),
            new HomeLocation(4,"Algeria"),
            new HomeLocation(5,"Azerbaijan"),
            new HomeLocation(6,"Albania"),
            new HomeLocation(7,"Armenia"),
            new HomeLocation(8,"Andorra"),
            new HomeLocation(9,"Angola"),
            new HomeLocation(10,"American Samoa"),
            new HomeLocation(11,"Argentina"),
            new HomeLocation(12,"Australia"),
            new HomeLocation(14,"Austria"),
            new HomeLocation(17,"Bahrain"),
            new HomeLocation(18,"Barbados"),
            new HomeLocation(19,"Botswana"),
            new HomeLocation(20,"Bermuda"),
            new HomeLocation(21,"Belgium"),
            new HomeLocation(22,"Bahamas, The"),
            new HomeLocation(23,"Bangladesh"),
            new HomeLocation(24,"Belize"),
            new HomeLocation(25,"Bosnia and Herzegovina"),
            new HomeLocation(26,"Bolivia"),
            new HomeLocation(27,"Myanmar "),
            new HomeLocation(28,"Benin"),
            new HomeLocation(29,"Belarus"),
            new HomeLocation(30,"Solomon Islands"),
            new HomeLocation(32,"Brazil"),
            new HomeLocation(34,"Bhutan"),
            new HomeLocation(35,"Bulgaria"),
            new HomeLocation(37,"Brunei"),
            new HomeLocation(38,"Burundi"),
            new HomeLocation(39,"Canada"),
            new HomeLocation(40,"Cambodia"),
            new HomeLocation(41,"Chad"),
            new HomeLocation(42,"Sri Lanka"),
            new HomeLocation(43,"Congo"),
            new HomeLocation(44,"Congo (DRC)"),
            new HomeLocation(45,"China"),
            new HomeLocation(46,"Chile"),
            new HomeLocation(49,"Cameroon"),
            new HomeLocation(50,"Comoros"),
            new HomeLocation(51,"Colombia"),
            new HomeLocation(54,"Costa Rica"),
            new HomeLocation(55,"Central African Republic"),
            new HomeLocation(56,"Cuba"),
            new HomeLocation(57,"Cabo Verde"),
            new HomeLocation(59,"Cyprus"),
            new HomeLocation(61,"Denmark"),
            new HomeLocation(62,"Djibouti"),
            new HomeLocation(63,"Dominica"),
            new HomeLocation(65,"Dominican Republic"),
            new HomeLocation(66,"Ecuador"),
            new HomeLocation(67,"Egypt"),
            new HomeLocation(68,"Ireland"),
            new HomeLocation(69,"Equatorial Guinea"),
            new HomeLocation(70,"Estonia"),
            new HomeLocation(71,"Eritrea"),
            new HomeLocation(72,"El Salvador"),
            new HomeLocation(73,"Ethiopia"),
            new HomeLocation(75,"Czech Republic"),
            new HomeLocation(77,"Finland"),
            new HomeLocation(78,"Fiji"),
            new HomeLocation(80,"Micronesia"),
            new HomeLocation(81,"Faroe Islands"),
            new HomeLocation(84,"France"),
            new HomeLocation(86,"Gambia"),
            new HomeLocation(87,"Gabon"),
            new HomeLocation(88,"Georgia"),
            new HomeLocation(89,"Ghana"),
            new HomeLocation(90,"Gibraltar"),
            new HomeLocation(91,"Grenada"),
            new HomeLocation(93,"Greenland"),
            new HomeLocation(94,"Germany"),
            new HomeLocation(98,"Greece"),
            new HomeLocation(99,"Guatemala"),
            new HomeLocation(100,"Guinea"),
            new HomeLocation(101,"Guyana"),
            new HomeLocation(103,"Haiti"),
            new HomeLocation(104,"Hong Kong SAR"),
            new HomeLocation(106,"Honduras"),
            new HomeLocation(108,"Croatia"),
            new HomeLocation(109,"Hungary"),
            new HomeLocation(110,"Iceland"),
            new HomeLocation(111,"Indonesia"),
            new HomeLocation(113,"India"),
            new HomeLocation(114,"British Indian Ocean Territory"),
            new HomeLocation(116,"Iran"),
            new HomeLocation(117,"Israel"),
            new HomeLocation(118,"Italy"),
            new HomeLocation(119,"Côte d'Ivoire"),
            new HomeLocation(121,"Iraq"),
            new HomeLocation(122,"Japan"),
            new HomeLocation(124,"Jamaica"),
            new HomeLocation(125,"Jan Mayen"),
            new HomeLocation(126,"Jordan"),
            new HomeLocation(127,"Johnston Atoll"),
            new HomeLocation(129,"Kenya"),
            new HomeLocation(130,"Kyrgyzstan"),
            new HomeLocation(131,"North Korea"),
            new HomeLocation(133,"Kiribati"),
            new HomeLocation(134,"Korea"),
            new HomeLocation(136,"Kuwait"),
            new HomeLocation(137,"Kazakhstan"),
            new HomeLocation(138,"Laos"),
            new HomeLocation(139,"Lebanon"),
            new HomeLocation(140,"Latvia"),
            new HomeLocation(141,"Lithuania"),
            new HomeLocation(142,"Liberia"),
            new HomeLocation(143,"Slovakia"),
            new HomeLocation(145,"Liechtenstein"),
            new HomeLocation(146,"Lesotho"),
            new HomeLocation(147,"Luxembourg"),
            new HomeLocation(148,"Libya"),
            new HomeLocation(149,"Madagascar"),
            new HomeLocation(151,"Macao SAR"),
            new HomeLocation(152,"Moldova"),
            new HomeLocation(154,"Mongolia"),
            new HomeLocation(156,"Malawi"),
            new HomeLocation(157,"Mali"),
            new HomeLocation(158,"Monaco"),
            new HomeLocation(159,"Morocco"),
            new HomeLocation(160,"Mauritius"),
            new HomeLocation(162,"Mauritania"),
            new HomeLocation(163,"Malta"),
            new HomeLocation(164,"Oman"),
            new HomeLocation(165,"Maldives"),
            new HomeLocation(166,"Mexico"),
            new HomeLocation(167,"Malaysia"),
            new HomeLocation(168,"Mozambique"),
            new HomeLocation(173,"Niger"),
            new HomeLocation(174,"Vanuatu"),
            new HomeLocation(175,"Nigeria"),
            new HomeLocation(176,"Netherlands"),
            new HomeLocation(177,"Norway"),
            new HomeLocation(178,"Nepal"),
            new HomeLocation(180,"Nauru"),
            new HomeLocation(181,"Suriname"),
            new HomeLocation(182,"Nicaragua"),
            new HomeLocation(183,"New Zealand"),
            new HomeLocation(184,"Palestinian Authority"),
            new HomeLocation(185,"Paraguay"),
            new HomeLocation(187,"Peru"),
            new HomeLocation(190,"Pakistan"),
            new HomeLocation(191,"Poland"),
            new HomeLocation(192,"Panama"),
            new HomeLocation(193,"Portugal"),
            new HomeLocation(194,"Papua New Guinea"),
            new HomeLocation(195,"Palau"),
            new HomeLocation(196,"Guinea-Bissau"),
            new HomeLocation(197,"Qatar"),
            new HomeLocation(198,"Réunion"),
            new HomeLocation(199,"Marshall Islands"),
            new HomeLocation(200,"Romania"),
            new HomeLocation(201,"Philippines"),
            new HomeLocation(202,"Puerto Rico"),
            new HomeLocation(203,"Russia"),
            new HomeLocation(204,"Rwanda"),
            new HomeLocation(205,"Saudi Arabia"),
            new HomeLocation(206,"Saint Pierre and Miquelon"),
            new HomeLocation(207,"Saint Kitts and Nevis"),
            new HomeLocation(208,"Seychelles"),
            new HomeLocation(209,"South Africa"),
            new HomeLocation(210,"Senegal"),
            new HomeLocation(212,"Slovenia"),
            new HomeLocation(213,"Sierra Leone"),
            new HomeLocation(214,"San Marino"),
            new HomeLocation(215,"Singapore"),
            new HomeLocation(216,"Somalia"),
            new HomeLocation(217,"Spain"),
            new HomeLocation(218,"Saint Lucia"),
            new HomeLocation(219,"Sudan"),
            new HomeLocation(220,"Svalbard"),
            new HomeLocation(221,"Sweden"),
            new HomeLocation(222,"Syria"),
            new HomeLocation(223,"Switzerland"),
            new HomeLocation(224,"United Arab Emirates"),
            new HomeLocation(225,"Trinidad and Tobago"),
            new HomeLocation(227,"Thailand"),
            new HomeLocation(228,"Tajikistan"),
            new HomeLocation(231,"Tonga"),
            new HomeLocation(232,"Togo"),
            new HomeLocation(233,"São Tomé and Príncipe"),
            new HomeLocation(234,"Tunisia"),
            new HomeLocation(235,"Turkey"),
            new HomeLocation(236,"Tuvalu"),
            new HomeLocation(237,"Taiwan"),
            new HomeLocation(238,"Turkmenistan"),
            new HomeLocation(239,"Tanzania"),
            new HomeLocation(240,"Uganda"),
            new HomeLocation(241,"Ukraine"),
            new HomeLocation(242,"United Kingdom"),
            new HomeLocation(244,"United States"),
            new HomeLocation(245,"Burkina Faso"),
            new HomeLocation(246,"Uruguay"),
            new HomeLocation(247,"Uzbekistan"),
            new HomeLocation(248,"Saint Vincent and the Grenadines"),
            new HomeLocation(249,"Venezuela"),
            new HomeLocation(251,"Vietnam"),
            new HomeLocation(252,"U.S. Virgin Islands"),
            new HomeLocation(253,"Vatican City"),
            new HomeLocation(254,"Namibia"),
            new HomeLocation(258,"Wake Island"),
            new HomeLocation(259,"Samoa"),
            new HomeLocation(260,"Swaziland"),
            new HomeLocation(261,"Yemen"),
            new HomeLocation(263,"Zambia"),
            new HomeLocation(264,"Zimbabwe"),
            new HomeLocation(269,"Serbia and Montenegro (Former)"),
            new HomeLocation(270,"Montenegro"),
            new HomeLocation(271,"Serbia"),
            new HomeLocation(273,"Curaçao"),
            new HomeLocation(300,"Anguilla"),
            new HomeLocation(276,"South Sudan"),
            new HomeLocation(301,"Antarctica"),
            new HomeLocation(302,"Aruba"),
            new HomeLocation(303,"Ascension Island"),
            new HomeLocation(304,"Ashmore and Cartier Islands"),
            new HomeLocation(305,"Baker Island"),
            new HomeLocation(306,"Bouvet Island"),
            new HomeLocation(307,"Cayman Islands"),
            new HomeLocation(308,"Channel Islands"),
            new HomeLocation(309,"Christmas Island"),
            new HomeLocation(310,"Clipperton Island"),
            new HomeLocation(311,"Cocos (Keeling) Islands"),
            new HomeLocation(312,"Cook Islands"),
            new HomeLocation(313,"Coral Sea Islands"),
            new HomeLocation(314,"Diego Garcia"),
            new HomeLocation(315,"Falkland Islands"),
            new HomeLocation(317,"French Guiana"),
            new HomeLocation(318,"French Polynesia"),
            new HomeLocation(319,"French Southern Territories"),
            new HomeLocation(321,"Guadeloupe"),
            new HomeLocation(322,"Guam"),
            new HomeLocation(323,"Guantanamo Bay"),
            new HomeLocation(324,"Guernsey"),
            new HomeLocation(325,"Heard Island and McDonald Islands"),
            new HomeLocation(326,"Howland Island"),
            new HomeLocation(327,"Jarvis Island"),
            new HomeLocation(328,"Jersey"),
            new HomeLocation(329,"Kingman Reef"),
            new HomeLocation(330,"Martinique"),
            new HomeLocation(331,"Mayotte"),
            new HomeLocation(332,"Montserrat"),
            new HomeLocation(333,"Netherlands Antilles (Former)"),
            new HomeLocation(334,"New Caledonia"),
            new HomeLocation(335,"Niue"),
            new HomeLocation(336,"Norfolk Island"),
            new HomeLocation(337,"Northern Mariana Islands"),
            new HomeLocation(338,"Palmyra Atoll"),
            new HomeLocation(339,"Pitcairn Islands"),
            new HomeLocation(340,"Rota Island"),
            new HomeLocation(341,"Saipan"),
            new HomeLocation(342,"South Georgia and the South Sandwich Islands"),
            new HomeLocation(343,"St Helena, Ascension and Tristan da Cunha"),
            new HomeLocation(346,"Tinian Island"),
            new HomeLocation(347,"Tokelau"),
            new HomeLocation(348,"Tristan da Cunha"),
            new HomeLocation(349,"Turks and Caicos Islands"),
            new HomeLocation(351,"British Virgin Islands"),
            new HomeLocation(352,"Wallis and Futuna"),
            new HomeLocation(742,"Africa"),
            new HomeLocation(2129,"Asia"),
            new HomeLocation(10541,"Europe"),
            new HomeLocation(15126,"Isle of Man"),
            new HomeLocation(19618,"Macedonia, FYRO"),
            new HomeLocation(20900,"Melanesia"),
            new HomeLocation(21206,"Micronesia"),
            new HomeLocation(21242,"Midway Islands"),
            new HomeLocation(23581,"Northern America"),
            new HomeLocation(26286,"Polynesia"),
            new HomeLocation(27082,"Central America"),
            new HomeLocation(27114,"Oceania"),
            new HomeLocation(30967,"Sint Maarten"),
            new HomeLocation(31396,"South America"),
            new HomeLocation(31706,"Saint Martin"),
            new HomeLocation(39070,"World"),
            new HomeLocation(42483,"Western Africa"),
            new HomeLocation(42484,"Middle Africa"),
            new HomeLocation(42487,"Northern Africa"),
            new HomeLocation(47590,"Central Asia"),
            new HomeLocation(47599,"South-Eastern Asia"),
            new HomeLocation(47600,"Eastern Asia"),
            new HomeLocation(47603,"Eastern Africa"),
            new HomeLocation(47609,"Eastern Europe"),
            new HomeLocation(47610,"Southern Europe"),
            new HomeLocation(47611,"Middle East"),
            new HomeLocation(47614,"Southern Asia"),
            new HomeLocation(7299303,"Timor-Leste"),
            new HomeLocation(9914689,"Kosovo"),
            new HomeLocation(10026358,"Americas"),
            new HomeLocation(10028789,"Åland Islands"),
            new HomeLocation(10039880,"Caribbean"),
            new HomeLocation(10039882,"Northern Europe"),
            new HomeLocation(10039883,"Southern Africa"),
            new HomeLocation(10210824,"Western Europe"),
            new HomeLocation(10210825,"Australia and New Zealand"),
            new HomeLocation(161832015,"Saint Barthélemy"),
            new HomeLocation(161832256,"U.S. Minor Outlying Islands"),
            new HomeLocation(161832257,"Latin America and the Caribbean"),
            new HomeLocation(161832258,"Bonaire, Sint Eustatius and Saba")
        };
        #endregion

        #region Papers
        List<PaperSizes> Papers = new List<PaperSizes>()
        {
            new PaperSizes(1,"US Letter 8 ½ x 11 in"),
            new PaperSizes(2,"US Letter Small 8 ½ x 11 in"),
            new PaperSizes(3,"US Tabloid 11 x 17 in"),
            new PaperSizes(4,"US Ledger 17 x 11 in "),
            new PaperSizes(5,"US Legal 8 ½ x 14 in"),
            new PaperSizes(6,"US Statement 5 ½ x 8 ½ in"),
            new PaperSizes(7,"US Executive 7 ¼ x 10 ½ in"),
            new PaperSizes(8,"A3 297 x 420 mm"),
            new PaperSizes(9,"A4 210 x 297 mm"),
            new PaperSizes(10,"A4 Small 210 x 297 mm"),
            new PaperSizes(11,"A5 148 x 210 mm"),
            new PaperSizes(12,"B4 (JIS) 250 x 354"),
            new PaperSizes(13,"B5 (JIS) 182 x 257 mm"),
            new PaperSizes(14,"Folio 8 ½ x 13 in"),
            new PaperSizes(15,"Quarto 215 x 275 mm"),
            new PaperSizes(16,"10 x 14 in"),
            new PaperSizes(17,"11 x 17 in"),
            new PaperSizes(18,"US Note 8 ½ x 11 in"),
            new PaperSizes(19,"US Envelope #9 3 ⅞ x 8 ⅞"),
            new PaperSizes(20,"US Envelope #10 4 ⅛ x 9 ½"),
            new PaperSizes(21,"US Envelope #11 4 ½ x 10 ⅜"),
            new PaperSizes(22,"US Envelope #12 4 ¾ x 11"),
            new PaperSizes(23,"US Envelope #14 5 x 11 ½"),
            new PaperSizes(24,"C size sheet"),
            new PaperSizes(25,"D size sheet"),
            new PaperSizes(26,"E size sheet"),
            new PaperSizes(27,"Envelope DL 110 x 220 mm"),
            new PaperSizes(28,"Envelope C5 162 x 229 mm"),
            new PaperSizes(29,"Envelope C3 324 x 458 mm"),
            new PaperSizes(30,"Envelope C4 229 x 324 mm"),
            new PaperSizes(31,"Envelope C6 114 x 162 mm"),
            new PaperSizes(32,"Envelope C65 114 x 229 mm"),
            new PaperSizes(33,"Envelope B4 250 x 353 mm"),
            new PaperSizes(34,"Envelope B5 176 x 250 mm"),
            new PaperSizes(35,"Envelope B6 176 x 125 mm"),
            new PaperSizes(36,"Envelope 110 x 230 mm"),
            new PaperSizes(37,"US Envelope Monarch 3.875 x 7.5 in"),
            new PaperSizes(38,"6 3/4 US Envelope 3 5/8 x 6 ½ in"),
            new PaperSizes(39,"US Std Fanfold 14 ⅞ x 11 in"),
            new PaperSizes(40,"German Std Fanfold 8 ½ x 12 in"),
            new PaperSizes(41,"German Legal Fanfold 8 ½ x 13 in"),
            new PaperSizes(42,"B4 (ISO) 250 x 353 mm"),
            new PaperSizes(43,"Japanese Postcard 100 x 148 mm"),
            new PaperSizes(44,"9 x 11 in"),
            new PaperSizes(45,"10 x 11 in"),
            new PaperSizes(46,"15 x 11 in"),
            new PaperSizes(47,"Envelope Invite 220 x 220 mm"),
            new PaperSizes(50,"US Letter Extra 9 ½ x 12 in"),
            new PaperSizes(51,"US Legal Extra 9 ½ x 15 in"),
            new PaperSizes(52,"US Tabloid Extra 11.69 x 18 in"),
            new PaperSizes(53,"A4 Extra 9.27 x 12.69 in"),
            new PaperSizes(54,"Letter Transverse 8 ½ x 11 in "),
            new PaperSizes(55,"A4 Transverse 210 x 297 mm"),
            new PaperSizes(56,"Letter Extra Transverse 9½ x 12 in"),
            new PaperSizes(57,"SuperA/SuperA/A4 227 x 356 mm"),
            new PaperSizes(58,"SuperB/SuperB/A3 305 x 487 mm"),
            new PaperSizes(59,"US Letter Plus 8.5 x 12.69 in"),
            new PaperSizes(60,"A4 Plus 210 x 330 mm"),
            new PaperSizes(61,"A5 Transverse 148 x 210 mm"),
            new PaperSizes(62,"B5 (JIS) Transverse 182 x 257 mm"),
            new PaperSizes(63,"A3 Extra 322 x 445 mm"),
            new PaperSizes(64,"A5 Extra 174 x 235 mm"),
            new PaperSizes(65,"B5 (ISO) Extra 201 x 276 mm"),
            new PaperSizes(66,"A2 420 x 594 mm"),
            new PaperSizes(67,"A3 Transverse 297 x 420 mm"),
            new PaperSizes(68,"A3 Extra Transverse 322 x 445 mm "),
            new PaperSizes(69,"Japanese Double Postcard 200 x 148 mm"),
            new PaperSizes(70,"A6 105 x 148 mm"),
            new PaperSizes(71,"Japanese Envelope Kaku #2"),
            new PaperSizes(72,"Japanese Envelope Kaku #3"),
            new PaperSizes(73,"Japanese Envelope Chou #3"),
            new PaperSizes(74,"Japanese Envelope Chou #4"),
            new PaperSizes(75,"Letter Rotated 11 x 8 ½ 11 in"),
            new PaperSizes(76,"A3 Rotated 420 x 297 mm"),
            new PaperSizes(77,"A4 Rotated 297 x 210 mm"),
            new PaperSizes(78,"A5 Rotated 210 x 148 mm"),
            new PaperSizes(79,"B4 (JIS) Rotated 364 x 257 mm"),
            new PaperSizes(80,"B5 (JIS) Rotated 257 x 182 mm"),
            new PaperSizes(81,"Japanese Postcard Rotated 148 x 100 mm"),
            new PaperSizes(82,"Double Japanese Postcard Rotated 148 x 200 mm"),
            new PaperSizes(83,"A6 Rotated 148 x 105 mm"),
            new PaperSizes(84,"Japanese Envelope Kaku #2 Rotated"),
            new PaperSizes(85,"Japanese Envelope Kaku #3 Rotated"),
            new PaperSizes(86,"Japanese Envelope Chou #3 Rotated"),
            new PaperSizes(87,"Japanese Envelope Chou #4 Rotated"),
            new PaperSizes(88,"B6 (JIS) 128 x 182 mm"),
            new PaperSizes(89,"B6 (JIS) Rotated 182 x 128 mm"),
            new PaperSizes(90,"12 x 11 in"),
            new PaperSizes(91,"Japanese Envelope You #4"),
            new PaperSizes(92,"Japanese Envelope You #4 Rotated"),
            new PaperSizes(93,"PRC 16K 146 x 215 mm"),
            new PaperSizes(94,"PRC 32K 97 x 151 mm"),
            new PaperSizes(95,"PRC 32K(Big) 97 x 151 mm"),
            new PaperSizes(96,"PRC Envelope #1 102 x 165 mm"),
            new PaperSizes(97,"PRC Envelope #2 102 x 176 mm"),
            new PaperSizes(98,"PRC Envelope #3 125 x 176 mm"),
            new PaperSizes(99,"PRC Envelope #4 110 x 208 mm"),
            new PaperSizes(100,"PRC Envelope #5 110 x 220 mm"),
            new PaperSizes(101,"PRC Envelope #6 120 x 230 mm"),
            new PaperSizes(102,"PRC Envelope #7 160 x 230 mm"),
            new PaperSizes(103,"PRC Envelope #8 120 x 309 mm"),
            new PaperSizes(104,"PRC Envelope #9 229 x 324 mm"),
            new PaperSizes(105,"PRC Envelope #10 324 x 458 mm"),
            new PaperSizes(106,"PRC 16K Rotated"),
            new PaperSizes(107,"PRC 32K Rotated"),
            new PaperSizes(108,"PRC 32K(Big) Rotated"),
            new PaperSizes(109,"PRC Envelope #1 Rotated 165 x 102 mm"),
            new PaperSizes(110,"PRC Envelope #2 Rotated 176 x 102 mm"),
            new PaperSizes(111,"PRC Envelope #3 Rotated 176 x 125 mm"),
            new PaperSizes(112,"PRC Envelope #4 Rotated 208 x 110 mm"),
            new PaperSizes(113,"PRC Envelope #5 Rotated 220 x 110 mm"),
            new PaperSizes(114,"PRC Envelope #6 Rotated 230 x 120 mm"),
            new PaperSizes(115,"PRC Envelope #7 Rotated 230 x 160 mm"),
            new PaperSizes(116,"PRC Envelope #8 Rotated 309 x 120 mm"),
            new PaperSizes(117,"PRC Envelope #9 Rotated 324 x 229 mm"),
            new PaperSizes(118,"PRC Envelope #10 Rotated 458 x 324 mm")
        };
        #endregion

        class TZI
        {
            public string Name;
            public string RegKeyName;
            public override string ToString()
            {
                return (Name);
            }
        }

        class KBI
        {
            public string Name;
            public string RegKeyName;
            public override string ToString()
            {
                return (Name);
            }
        }

        class FoxCultureInfo
        {
            public string Name;
            public int ID;
            public override string ToString()
            {
                return (Name);
            }
        }

        class PaperSizes
        {
            public string Name;
            public int ID;
            public override string ToString()
            {
                return (Name);
            }

            public PaperSizes(int ID, string Name)
            {
                this.ID = ID;
                this.Name = Name;
            }

            public PaperSizes()
            {

            }
        }

        public ctlIntl()
        {
            InitializeComponent();
        }

        public string GetData()
        {
            IntlSettings.EnableCurrDecimalSymbol = C(chkCurrDecSymbol.CheckState);
            IntlSettings.EnableCurrDigitGrouping = C(chkCurrDigitGroup.CheckState);
            IntlSettings.EnableCurrDigitGroupingSymbol = C(chkCurrDigitGroupSymbol.CheckState);
            IntlSettings.EnableCurrNegativeFormat = C(chkCurrNegFormat.CheckState);
            IntlSettings.EnableCurrNumDigitsAfterDec = C(chkCurrNumDigits.CheckState);
            IntlSettings.EnableCurrPositiveFormat = C(chkCurrPosFormat.CheckState);
            IntlSettings.EnableCurrCurrencySymbol = C(chkCurrSymbol.CheckState);
            IntlSettings.EnableDate1stDayOfWeek = C(chkDate1stWeek.CheckState);
            IntlSettings.EnableDate2DigitYear = C(chkDate2DigitYear.CheckState);
            IntlSettings.EnableDateLongDate = C(chkDateLongDate.CheckState);
            IntlSettings.EnableDateShortDate = C(chkDateShortDate.CheckState);
            IntlSettings.EnableDateFormat = C(chkDateFormat.CheckState);
            IntlSettings.EnableMainFormat = C(chkFormat.CheckState);
            IntlSettings.EnableKeyboardLayout1 = C(chkKeybLayout1.CheckState);
            IntlSettings.EnableKeyboardLayout2 = C(chkKeybLayout2.CheckState);
            IntlSettings.EnableKeyboardLayout3 = C(chkKeybLayout3.CheckState);
            IntlSettings.EnableMainLocation = C(chkLocation.CheckState);
            IntlSettings.EnableNumDecimalSymbol = C(chkNumDecSymbol.CheckState);
            IntlSettings.EnableNumDigitGrouping = C(chkNumDigitGroup.CheckState);
            IntlSettings.EnableNumDigitGroupingSymbol = C(chkNumDigitGroupSymbol.CheckState);
            IntlSettings.EnableNumDisplayLeading0 = C(chkNumDispLeading0.CheckState);
            IntlSettings.EnableNumListSeparator = C(chkNumListSeparator.CheckState);
            IntlSettings.EnableNumMeasurement = C(chkNumMeasure.CheckState);
            IntlSettings.EnableNumUseNativeDigits = C(chkNumNativeDigits.CheckState);
            IntlSettings.EnableNumNegativeNumberFormat = C(chkNumNegNumFormat.CheckState);
            IntlSettings.EnableNumNegativeSignSymbol = C(chkNumNegSignSymbol.CheckState);
            IntlSettings.EnableNumPositiveSignSymbol = C(chkNumPosSignSymbol.CheckState);
            IntlSettings.EnableNumNumOfDigitsAfterDec = C(chkNumNumDigits.CheckState);
            IntlSettings.EnableNumStdDigits = C(chkNumStdDigits.CheckState);
            IntlSettings.EnableMainSystemLocale = C(chkSysLocale.CheckState);
            IntlSettings.EnableTelephoneIDN = C(chkTelephone.CheckState);
            IntlSettings.EnableTimeAM = C(chkTimeAM.CheckState);
            IntlSettings.EnableTimeLongTime = C(chkTimeLongTime.CheckState);
            IntlSettings.EnableTimePM = C(chkTimePM.CheckState);
            IntlSettings.EnableTimeZone = C(chkTimeZone.CheckState);
            IntlSettings.EnableTimeShortTime = C(chkTimShortTime.CheckState);
            IntlSettings.DeleteOtherKeyboardLayouts = C(chkKeyboardDelete.CheckState);
            IntlSettings.AutoDST = C(chkDST.CheckState);
            IntlSettings.EnableTime24Hour = C(chkTime24Hour.CheckState);
            IntlSettings.EnableTimeShortPrefix0Hour = C(chkTimeShortPrefix0Hour.CheckState);
            IntlSettings.EnableDateFirstWeekOfYear = C(chkDateFirstWeekOfYear.CheckState);
            IntlSettings.EnableDateSeparator = C(chkDateSeparator.CheckState);
            IntlSettings.EnablePaperSize = C(chkPaperSize.CheckState);
            IntlSettings.EnableTimeSeparator = C(chkTimeSeparator.CheckState);

            IntlSettings.CurrDigitGrouping = lstCurrDigitGroup.SelectedIndex;
            IntlSettings.CurrNegativeFormat = lstCurrNegFormat.SelectedIndex;
            IntlSettings.CurrNumDigitsAfterDec = lstCurrNumDigits.SelectedIndex;
            IntlSettings.CurrPositiveFormat = lstCurrPosFormat.SelectedIndex;
            IntlSettings.Date1stDayOfWeek = lstDate1stWeek.SelectedIndex;
            IntlSettings.NumDigitGrouping = lstNumDigitGroup.SelectedIndex;
            IntlSettings.NumDisplayLeading0 = lstNumDispLeading0.SelectedIndex;
            IntlSettings.NumMeasurement = lstNumMeasure.SelectedIndex;
            IntlSettings.NumUseNativeDigits = lstNumNativeDigits.SelectedIndex;
            IntlSettings.NumNegativeNumberFormat = lstNumNegNumFormat.SelectedIndex;
            IntlSettings.NumNumOfDigitsAfterDec = lstNumNumDigits.SelectedIndex;
            IntlSettings.NumStdDigits = lstNumStdDigits.SelectedIndex;
            IntlSettings.CurrDecimalSymbol = txtCurrDecSymbol.Text;
            IntlSettings.CurrDigitGroupingSymbol = txtCurrDigitGroupSymbol.Text;
            IntlSettings.CurrCurrencySymbol = txtCurrSymbol.Text;
            IntlSettings.DateLongDate = txtDateLongDate.Text;
            IntlSettings.DateShortDate = txtDateShortDate.Text;
            IntlSettings.DateFormat = lstDateFormat.SelectedIndex;
            IntlSettings.NumDecimalSymbol = txtNumDecSymbol.Text;
            IntlSettings.NumDigitGroupingSymbol = txtNumDigitGroupSymbol.Text;
            IntlSettings.NumListSeparator = txtNumListSeparator.Text;
            IntlSettings.NumNegativeSignSymbol = txtNumNegSignSymbol.Text;
            IntlSettings.NumPositiveSignSymbol = txtNumPosSignSymbol.Text;
            IntlSettings.TelephoneIDN = txtTelephone.Text;
            IntlSettings.TimeAM = txtTimeAM.Text;
            IntlSettings.TimeLongTime = txtTimeLongTime.Text;
            IntlSettings.TimePM = txtTimePM.Text;
            IntlSettings.TimeShortTime = txtTimeShortTime.Text;
            IntlSettings.Time24Hour = lstTime24Hour.SelectedIndex;
            IntlSettings.TimeShortTimePrefix0Hour = lstTimeShortPrefix0Hour.SelectedIndex;
            IntlSettings.DateFirstWeekOfYear = lstDateFirstWeekOfYear.SelectedIndex;
            IntlSettings.DateSeparator = txtDateSeparator.Text;
            IntlSettings.TimeSeparator = txtTimeSeparator.Text;

            int y;
            if (int.TryParse(txtDate2DigitYearTo.Text, out y) == false)
                IntlSettings.Date2DigitYear = 2057;
            else
                IntlSettings.Date2DigitYear = y;

            IntlSettings.KeyboardLayout1 = ((KBI)lstKeybLayout1.SelectedItem).RegKeyName;
            IntlSettings.KeyboardLayout2 = ((KBI)lstKeybLayout2.SelectedItem).RegKeyName;
            IntlSettings.KeyboardLayout3 = ((KBI)lstKeybLayout3.SelectedItem).RegKeyName;

            IntlSettings.MainFormat = ((FoxCultureInfo)lstFormat.SelectedItem).ID;
            IntlSettings.MainLocation = ((HomeLocation)lstLocation.SelectedItem).ID;
            IntlSettings.MainSystemLocale = ((FoxCultureInfo)lstSysLocale.SelectedItem).ID;
            IntlSettings.TimeZone = ((TZI)lstTimeZone.SelectedItem).RegKeyName;
            IntlSettings.PaperSize = ((PaperSizes)lstPaperSize.SelectedItem).ID;

            return (JsonConvert.SerializeObject(IntlSettings));
        }

        public bool SetData(PolicyObject obj)
        {
            Pol = obj;

            IntlSettings = JsonConvert.DeserializeObject<InternationalPolicy>(obj.Data);
            if (IntlSettings == null)
            {
                IntlSettings = new InternationalPolicy();
                //Setup defaults
                IntlSettings.CurrCurrencySymbol = "€";
                IntlSettings.CurrDecimalSymbol = ".";
                IntlSettings.CurrDigitGrouping = 1;
                IntlSettings.CurrDigitGroupingSymbol = "'";
                IntlSettings.CurrNegativeFormat = 5;
                IntlSettings.CurrNumDigitsAfterDec = 2;
                IntlSettings.CurrPositiveFormat = 1;
                IntlSettings.Date1stDayOfWeek = 0;
                IntlSettings.Date2DigitYear = 2039;
                IntlSettings.DateLongDate = "dddd, dd MMMM yyyy";
                IntlSettings.DateShortDate = "dd/MM/yyyy";
                IntlSettings.KeyboardLayout1 = "00000807";
                IntlSettings.KeyboardLayout2 = "00000409";
                IntlSettings.KeyboardLayout3 = "00000409";
                IntlSettings.MainFormat = 2057;
                IntlSettings.MainLocation = 147;
                IntlSettings.MainSystemLocale = 2057;
                IntlSettings.NumDecimalSymbol = ".";
                IntlSettings.NumDigitGrouping = 1;
                IntlSettings.NumDigitGroupingSymbol = "'";
                IntlSettings.NumDisplayLeading0 = 1;
                IntlSettings.NumListSeparator = ";";
                IntlSettings.NumMeasurement = 0;
                IntlSettings.NumNegativeNumberFormat = 1;
                IntlSettings.NumNegativeSignSymbol = "-";
                IntlSettings.NumNumOfDigitsAfterDec = 2;
                IntlSettings.NumStdDigits = 0;
                IntlSettings.NumUseNativeDigits = 1;
                IntlSettings.TelephoneIDN = "352";
                IntlSettings.TimeAM = "AM";
                IntlSettings.TimePM = "PM";
                IntlSettings.TimeLongTime = "HH:mm:ss";
                IntlSettings.TimeShortTime = "HH:mm";
                IntlSettings.TimeZone = "W. Europe Standard Time";
                IntlSettings.PaperSize = 9;
                IntlSettings.DateSeparator = "/";
                IntlSettings.DateFirstWeekOfYear = 1;
                IntlSettings.Time24Hour = 1;
                IntlSettings.DateFormat = 1;
                IntlSettings.TimeSeparator = ":";
            }

            return (true);
        }

        private void Intl_Load(object sender, EventArgs e)
        {
            lblName.Text = Pol.Name;

            RegistryKey tz = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones", false);
            if (tz != null)
            {
                foreach (string r in tz.GetSubKeyNames())
                {
                    TZI tzi = new TZI();
                    tzi.RegKeyName = r;
                    RegistryKey tzz = tz.OpenSubKey(r, false);
                    if (tzz != null)
                    {
                        tzi.Name = tzz.GetValue("Display").ToString();
                        tzz.Close();
                    }
                    lstTimeZone.Items.Add(tzi);
                }
                tz.Close();
            }

            RegistryKey kb = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layouts", false);
            if (kb != null)
            {
                foreach (string r in kb.GetSubKeyNames())
                {
                    KBI kbi = new KBI();
                    kbi.RegKeyName = r;
                    RegistryKey tzz = kb.OpenSubKey(r, false);
                    if (tzz != null)
                    {
                        kbi.Name = tzz.GetValue("Layout Text").ToString();
                        tzz.Close();
                    }
                    lstKeybLayout1.Items.Add(kbi);
                    lstKeybLayout2.Items.Add(kbi);
                    lstKeybLayout3.Items.Add(kbi);
                }
                kb.Close();
            }

            lstLocation.Items.AddRange(hloc.ToArray());
            lstPaperSize.Items.AddRange(Papers.ToArray());

            foreach (CultureInfo cult in CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures))
            {
                if (cult.LCID == 127)
                    continue;
                FoxCultureInfo c = new FoxCultureInfo();
                c.Name = cult.EnglishName;
                c.ID = cult.LCID;
                lstFormat.Items.Add(c);
                lstSysLocale.Items.Add(c);
            }

            for (int i = 0; i < 10; i++)
            {
                lstNumNumDigits.Items.Add(i.ToString());
                lstCurrNumDigits.Items.Add(i.ToString());
            }

            lstNumDigitGroup.Items.Add("123456789");
            lstNumDigitGroup.Items.Add("123'456'789");
            lstNumDigitGroup.Items.Add("123456'789");
            lstNumDigitGroup.Items.Add("12'34'56'789");
            lstCurrDigitGroup.Items.Add("123456789");
            lstCurrDigitGroup.Items.Add("123'456'789");
            lstCurrDigitGroup.Items.Add("123456'789");
            lstCurrDigitGroup.Items.Add("12'34'56'789");

            lstNumNegNumFormat.Items.Add("(1.1)");
            lstNumNegNumFormat.Items.Add("-1.1");
            lstNumNegNumFormat.Items.Add("- 1.1");
            lstNumNegNumFormat.Items.Add("1.1-");
            lstNumNegNumFormat.Items.Add("1.1 -");

            lstNumDispLeading0.Items.Add(".7");
            lstNumDispLeading0.Items.Add("0.7");

            lstNumMeasure.Items.Add("Metric");
            lstNumMeasure.Items.Add("U.S.");

            lstNumStdDigits.Items.Add("0123456789");

            lstNumNativeDigits.Items.Add("Context");
            lstNumNativeDigits.Items.Add("Never");
            lstNumNativeDigits.Items.Add("National");

            lstCurrPosFormat.Items.Add("¤1.1");
            lstCurrPosFormat.Items.Add("1.1¤");
            lstCurrPosFormat.Items.Add("¤ 1.1");
            lstCurrPosFormat.Items.Add("1.1 ¤");

            lstCurrNegFormat.Items.Add("(¤1.1)");
            lstCurrNegFormat.Items.Add("-¤1.1");
            lstCurrNegFormat.Items.Add("¤-1.1");
            lstCurrNegFormat.Items.Add("¤1.1-");
            lstCurrNegFormat.Items.Add("(1.1¤)");
            lstCurrNegFormat.Items.Add("-1.1¤");
            lstCurrNegFormat.Items.Add("1.1-¤");
            lstCurrNegFormat.Items.Add("1.1¤-");
            lstCurrNegFormat.Items.Add("-1.1 ¤");
            lstCurrNegFormat.Items.Add("-¤ 1.1");
            lstCurrNegFormat.Items.Add("1.1 ¤-");
            lstCurrNegFormat.Items.Add("¤ 1.1-");
            lstCurrNegFormat.Items.Add("¤ -1.1");
            lstCurrNegFormat.Items.Add("1.1- ¤");
            lstCurrNegFormat.Items.Add("(¤ 1.1)");
            lstCurrNegFormat.Items.Add("(1.1 ¤)");

            lstDate1stWeek.Items.Add("Monday");
            lstDate1stWeek.Items.Add("Tuesday");
            lstDate1stWeek.Items.Add("Wednesday");
            lstDate1stWeek.Items.Add("Thursday");
            lstDate1stWeek.Items.Add("Friday");
            lstDate1stWeek.Items.Add("Saturday");
            lstDate1stWeek.Items.Add("Sunday");

            lstDateFormat.Items.Add("mm/dd/yy");
            lstDateFormat.Items.Add("dd/mm/yy");
            lstDateFormat.Items.Add("yy/mm/dd");

            lstTime24Hour.Items.Add("12 hour clock");
            lstTime24Hour.Items.Add("24 hour clock");

            lstTimeShortPrefix0Hour.Items.Add("6:04");
            lstTimeShortPrefix0Hour.Items.Add("06:04");

            lstDateFirstWeekOfYear.Items.Add("Week containing January 1 is the first week of the year");
            lstDateFirstWeekOfYear.Items.Add("First full week following January 1 is the first week of the year");
            lstDateFirstWeekOfYear.Items.Add("First week containing at least 4 days is the first week of the year");

            LoadSettings();
            UpdateUI();
        }

        void UpdateUI()
        {
            lstCurrDigitGroup.Enabled = chkCurrDigitGroup.CheckState == CheckState.Checked ? true : false;
            lstCurrNegFormat.Enabled = chkCurrNegFormat.CheckState == CheckState.Checked ? true : false;
            lstCurrNumDigits.Enabled = chkCurrNumDigits.CheckState == CheckState.Checked ? true : false;
            lstCurrPosFormat.Enabled = chkCurrPosFormat.CheckState == CheckState.Checked ? true : false;
            lstDate1stWeek.Enabled = chkDate1stWeek.CheckState == CheckState.Checked ? true : false;
            lstFormat.Enabled = chkFormat.CheckState == CheckState.Checked ? true : false;
            lstKeybLayout1.Enabled = chkKeybLayout1.CheckState == CheckState.Checked ? true : false;
            lstKeybLayout2.Enabled = chkKeybLayout2.CheckState == CheckState.Checked ? true : false;
            lstKeybLayout3.Enabled = chkKeybLayout3.CheckState == CheckState.Checked ? true : false;
            lstLocation.Enabled = chkLocation.CheckState == CheckState.Checked ? true : false;
            lstNumDigitGroup.Enabled = chkNumDigitGroup.CheckState == CheckState.Checked ? true : false;
            lstNumDispLeading0.Enabled = chkNumDispLeading0.CheckState == CheckState.Checked ? true : false;
            lstNumMeasure.Enabled = chkNumMeasure.CheckState == CheckState.Checked ? true : false;
            lstNumNativeDigits.Enabled = chkNumNativeDigits.CheckState == CheckState.Checked ? true : false;
            lstNumNegNumFormat.Enabled = chkNumNegNumFormat.CheckState == CheckState.Checked ? true : false;
            lstNumNumDigits.Enabled = chkNumNumDigits.CheckState == CheckState.Checked ? true : false;
            lstNumStdDigits.Enabled = chkNumStdDigits.CheckState == CheckState.Checked ? true : false;
            lstSysLocale.Enabled = chkSysLocale.CheckState == CheckState.Checked ? true : false;
            lstTimeZone.Enabled = chkTimeZone.CheckState == CheckState.Checked ? true : false;
            chkDST.Enabled = chkTimeZone.CheckState == CheckState.Checked ? true : false;
            txtCurrDecSymbol.Enabled = chkCurrDecSymbol.CheckState == CheckState.Checked ? true : false;
            txtCurrDigitGroupSymbol.Enabled = chkCurrDigitGroupSymbol.CheckState == CheckState.Checked ? true : false;
            txtCurrSymbol.Enabled = chkCurrSymbol.CheckState == CheckState.Checked ? true : false;
            txtDate2DigitYearFrom.Enabled = chkDate2DigitYear.CheckState == CheckState.Checked ? true : false;
            txtDate2DigitYearTo.Enabled = chkDate2DigitYear.CheckState == CheckState.Checked ? true : false;
            txtDateLongDate.Enabled = chkDateLongDate.CheckState == CheckState.Checked ? true : false;
            txtDateShortDate.Enabled = chkDateShortDate.CheckState == CheckState.Checked ? true : false;
            txtNumDecSymbol.Enabled = chkNumDecSymbol.CheckState == CheckState.Checked ? true : false;
            txtNumDigitGroupSymbol.Enabled = chkNumDigitGroupSymbol.CheckState == CheckState.Checked ? true : false;
            txtNumListSeparator.Enabled = chkNumListSeparator.CheckState == CheckState.Checked ? true : false;
            txtNumNegSignSymbol.Enabled = chkNumNegSignSymbol.CheckState == CheckState.Checked ? true : false;
            txtNumPosSignSymbol.Enabled = chkNumPosSignSymbol.CheckState == CheckState.Checked ? true : false;
            txtTimeAM.Enabled = chkTimeAM.CheckState == CheckState.Checked ? true : false;
            txtTimeLongTime.Enabled = chkTimeLongTime.CheckState == CheckState.Checked ? true : false;
            txtTimePM.Enabled = chkTimePM.CheckState == CheckState.Checked ? true : false;
            txtTimeShortTime.Enabled = chkTimShortTime.CheckState == CheckState.Checked ? true : false;
            txtTelephone.Enabled = chkTelephone.CheckState == CheckState.Checked ? true : false;
            lstDateFormat.Enabled = chkDateFormat.CheckState == CheckState.Checked ? true : false;
            lstTimeShortPrefix0Hour.Enabled = chkTimeShortPrefix0Hour.CheckState == CheckState.Checked ? true : false;
            lstTime24Hour.Enabled = chkTime24Hour.CheckState == CheckState.Checked ? true : false;
            lstDateFirstWeekOfYear.Enabled = chkDateFirstWeekOfYear.CheckState == CheckState.Checked ? true : false;
            txtDateSeparator.Enabled = chkDateSeparator.CheckState == CheckState.Checked ? true : false;
            lstPaperSize.Enabled = chkPaperSize.CheckState == CheckState.Checked ? true : false;
            txtTimeSeparator.Enabled = chkTimeSeparator.CheckState == CheckState.Checked ? true : false;
        }

        void LoadSettings()
        {
            chkCurrDecSymbol.CheckState = C(IntlSettings.EnableCurrDecimalSymbol);
            chkCurrDigitGroup.CheckState = C(IntlSettings.EnableCurrDigitGrouping);
            chkCurrDigitGroupSymbol.CheckState = C(IntlSettings.EnableCurrDigitGroupingSymbol);
            chkCurrNegFormat.CheckState = C(IntlSettings.EnableCurrNegativeFormat);
            chkCurrNumDigits.CheckState = C(IntlSettings.EnableCurrNumDigitsAfterDec);
            chkCurrPosFormat.CheckState = C(IntlSettings.EnableCurrPositiveFormat);
            chkCurrSymbol.CheckState = C(IntlSettings.EnableCurrCurrencySymbol);
            chkDate1stWeek.CheckState = C(IntlSettings.EnableDate1stDayOfWeek);
            chkDate2DigitYear.CheckState = C(IntlSettings.EnableDate2DigitYear);
            chkDateLongDate.CheckState = C(IntlSettings.EnableDateLongDate);
            chkDateShortDate.CheckState = C(IntlSettings.EnableDateShortDate);
            chkFormat.CheckState = C(IntlSettings.EnableMainFormat);
            chkKeybLayout1.CheckState = C(IntlSettings.EnableKeyboardLayout1);
            chkKeybLayout2.CheckState = C(IntlSettings.EnableKeyboardLayout2);
            chkKeybLayout3.CheckState = C(IntlSettings.EnableKeyboardLayout3);
            chkLocation.CheckState = C(IntlSettings.EnableMainLocation);
            chkNumDecSymbol.CheckState = C(IntlSettings.EnableNumDecimalSymbol);
            chkNumDigitGroup.CheckState = C(IntlSettings.EnableNumDigitGrouping);
            chkNumDigitGroupSymbol.CheckState = C(IntlSettings.EnableNumDigitGroupingSymbol);
            chkNumDispLeading0.CheckState = C(IntlSettings.EnableNumDisplayLeading0);
            chkNumListSeparator.CheckState = C(IntlSettings.EnableNumListSeparator);
            chkNumMeasure.CheckState = C(IntlSettings.EnableNumMeasurement);
            chkNumNativeDigits.CheckState = C(IntlSettings.EnableNumUseNativeDigits);
            chkNumNegNumFormat.CheckState = C(IntlSettings.EnableNumNegativeNumberFormat);
            chkNumNegSignSymbol.CheckState = C(IntlSettings.EnableNumNegativeSignSymbol);
            chkNumPosSignSymbol.CheckState = C(IntlSettings.EnableNumPositiveSignSymbol);
            chkNumNumDigits.CheckState = C(IntlSettings.EnableNumNumOfDigitsAfterDec);
            chkNumStdDigits.CheckState = C(IntlSettings.EnableNumStdDigits);
            chkSysLocale.CheckState = C(IntlSettings.EnableMainSystemLocale);
            chkTelephone.CheckState = C(IntlSettings.EnableTelephoneIDN);
            chkTimeAM.CheckState = C(IntlSettings.EnableTimeAM);
            chkTimeLongTime.CheckState = C(IntlSettings.EnableTimeLongTime);
            chkTimePM.CheckState = C(IntlSettings.EnableTimePM);
            chkTimeZone.CheckState = C(IntlSettings.EnableTimeZone);
            chkTimShortTime.CheckState = C(IntlSettings.EnableTimeShortTime);
            chkKeyboardDelete.CheckState = C(IntlSettings.DeleteOtherKeyboardLayouts);
            chkDST.CheckState = C(IntlSettings.AutoDST);
            chkDateFormat.CheckState = C(IntlSettings.EnableDateFormat);
            chkTime24Hour.CheckState = C(IntlSettings.EnableTime24Hour);
            chkTimeShortPrefix0Hour.CheckState = C(IntlSettings.EnableTimeShortPrefix0Hour);
            chkDateFirstWeekOfYear.CheckState = C(IntlSettings.EnableDateFirstWeekOfYear);
            chkDateSeparator.CheckState = C(IntlSettings.EnableDateSeparator);
            chkPaperSize.CheckState = C(IntlSettings.EnablePaperSize);
            chkTimeSeparator.CheckState = C(IntlSettings.EnableTimeSeparator);

            lstCurrDigitGroup.SelectedIndex = IntlSettings.CurrDigitGrouping;
            lstCurrNegFormat.SelectedIndex = IntlSettings.CurrNegativeFormat;
            lstCurrNumDigits.SelectedIndex = IntlSettings.CurrNumDigitsAfterDec;
            lstCurrPosFormat.SelectedIndex = IntlSettings.CurrPositiveFormat;
            lstDate1stWeek.SelectedIndex = IntlSettings.Date1stDayOfWeek;
            lstNumDigitGroup.SelectedIndex = IntlSettings.NumDigitGrouping;
            lstNumDispLeading0.SelectedIndex = IntlSettings.NumDisplayLeading0;
            lstNumMeasure.SelectedIndex = IntlSettings.NumMeasurement;
            lstNumNativeDigits.SelectedIndex = IntlSettings.NumUseNativeDigits;
            lstNumNegNumFormat.SelectedIndex = IntlSettings.NumNegativeNumberFormat;
            lstNumNumDigits.SelectedIndex = IntlSettings.NumNumOfDigitsAfterDec;
            lstNumStdDigits.SelectedIndex = IntlSettings.NumStdDigits;
            lstDateFormat.SelectedIndex = IntlSettings.DateFormat;
            lstTimeShortPrefix0Hour.SelectedIndex = IntlSettings.TimeShortTimePrefix0Hour;
            lstTime24Hour.SelectedIndex = IntlSettings.Time24Hour;
            txtCurrDecSymbol.Text = IntlSettings.CurrDecimalSymbol;
            txtCurrDigitGroupSymbol.Text = IntlSettings.CurrDigitGroupingSymbol;
            txtCurrSymbol.Text = IntlSettings.CurrCurrencySymbol;
            txtDate2DigitYearTo.Text = IntlSettings.Date2DigitYear.ToString();
            txtDateLongDate.Text = IntlSettings.DateLongDate;
            txtDateShortDate.Text = IntlSettings.DateShortDate;
            txtNumDecSymbol.Text = IntlSettings.NumDecimalSymbol;
            txtNumDigitGroupSymbol.Text = IntlSettings.NumDigitGroupingSymbol;
            txtNumListSeparator.Text = IntlSettings.NumListSeparator;
            txtNumNegSignSymbol.Text = IntlSettings.NumNegativeSignSymbol;
            txtNumPosSignSymbol.Text = IntlSettings.NumPositiveSignSymbol;
            txtTelephone.Text = IntlSettings.TelephoneIDN;
            txtTimeAM.Text = IntlSettings.TimeAM;
            txtTimeLongTime.Text = IntlSettings.TimeLongTime;
            txtTimePM.Text = IntlSettings.TimePM;
            txtTimeShortTime.Text = IntlSettings.TimeShortTime;
            lstDateFirstWeekOfYear.SelectedIndex = IntlSettings.DateFirstWeekOfYear;
            txtDateSeparator.Text = IntlSettings.DateSeparator;
            txtTimeSeparator.Text = IntlSettings.TimeSeparator;

            lstKeybLayout1.SelectedIndex = FindKeyboard(IntlSettings.KeyboardLayout1);
            lstKeybLayout2.SelectedIndex = FindKeyboard(IntlSettings.KeyboardLayout2);
            lstKeybLayout3.SelectedIndex = FindKeyboard(IntlSettings.KeyboardLayout3);
            lstFormat.SelectedIndex = FindFormat(IntlSettings.MainFormat);
            lstSysLocale.SelectedIndex = FindFormat(IntlSettings.MainSystemLocale);
            lstLocation.SelectedIndex = FindLocation(IntlSettings.MainLocation);
            lstTimeZone.SelectedIndex = FindTimeZone(IntlSettings.TimeZone);
            lstPaperSize.SelectedIndex = FindPaper(IntlSettings.PaperSize);
        }

        int FindTimeZone(string TZ)
        {
            for (int i = 0; i < lstTimeZone.Items.Count; i++)
            {
                TZI cult = (TZI)lstTimeZone.Items[i];
                if (cult.RegKeyName == TZ)
                    return (i);
            }

            TZI c = new TZI();
            c.RegKeyName = TZ;
            c.Name = "??? " + TZ.ToString();
            lstTimeZone.Items.Add(c);
            return (lstTimeZone.Items.Count - 1);
        }

        int FindLocation(int Location)
        {
            for (int i = 0; i < lstLocation.Items.Count; i++)
            {
                HomeLocation cult = (HomeLocation)lstLocation.Items[i];
                if (cult.ID == Location)
                    return (i);
            }

            HomeLocation c = new HomeLocation();
            c.ID = Location;
            c.Name = "??? " + Location.ToString();
            lstLocation.Items.Add(c);
            return (lstLocation.Items.Count - 1);
        }

        int FindPaper(int Paper)
        {
            for (int i = 0; i < lstPaperSize.Items.Count; i++)
            {
                PaperSizes cult = (PaperSizes)lstPaperSize.Items[i];
                if (cult.ID == Paper)
                    return (i);
            }

            PaperSizes c = new PaperSizes();
            c.ID = Paper;
            c.Name = "??? " + Paper.ToString();
            lstPaperSize.Items.Add(c);
            return (lstPaperSize.Items.Count - 1);
        }

        int FindFormat(int Format)
        {
            for (int i = 0; i < lstFormat.Items.Count; i++)
            {
                FoxCultureInfo cult = (FoxCultureInfo)lstFormat.Items[i];
                if (cult.ID == Format)
                    return (i);
            }

            FoxCultureInfo c = new FoxCultureInfo();
            c.ID = Format;
            c.Name = "??? " + Format.ToString();
            lstFormat.Items.Add(c);
            lstSysLocale.Items.Add(c);
            return (lstFormat.Items.Count - 1);
        }

        int FindKeyboard(string Keyb)
        {
            for (int i = 0; i < lstKeybLayout1.Items.Count; i++)
            {
                KBI kb = (KBI)lstKeybLayout1.Items[i];
                if (kb.RegKeyName == Keyb)
                    return (i);
            }

            KBI kkb = new KBI();
            kkb.Name = "??? " + Keyb;
            kkb.RegKeyName = Keyb;
            lstKeybLayout1.Items.Add(kkb);
            lstKeybLayout2.Items.Add(kkb);
            lstKeybLayout3.Items.Add(kkb);
            return (lstKeybLayout1.Items.Count - 1);
        }

        private void chkMULTI_CheckStateChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string d = GetData();
            Program.net.EditPolicy(Pol.ID, d);
        }

        private void txtDate2DigitYearTo_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(txtDate2DigitYearTo.Text, out n) == false)
            {
                txtDate2DigitYearFrom.Text = "???";
            }
            else
            {
                txtDate2DigitYearFrom.Text = (n - 99).ToString();
            }
        }
    }
}
