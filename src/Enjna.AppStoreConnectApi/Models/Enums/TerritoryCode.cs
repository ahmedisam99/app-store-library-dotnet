using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The ISO 3166-1 alpha-3 code of an App Store territory.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/territorycode"/>
[JsonConverter(typeof(JsonEnumMemberConverter<TerritoryCode>))]
public enum TerritoryCode
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The territory code for Aruba.
    /// </summary>
    [EnumMember(Value = "ABW")]
    Abw,

    /// <summary>
    /// The territory code for Afghanistan.
    /// </summary>
    [EnumMember(Value = "AFG")]
    Afg,

    /// <summary>
    /// The territory code for Angola.
    /// </summary>
    [EnumMember(Value = "AGO")]
    Ago,

    /// <summary>
    /// The territory code for Anguilla.
    /// </summary>
    [EnumMember(Value = "AIA")]
    Aia,

    /// <summary>
    /// The territory code for Albania.
    /// </summary>
    [EnumMember(Value = "ALB")]
    Alb,

    /// <summary>
    /// The territory code for Andorra.
    /// </summary>
    [EnumMember(Value = "AND")]
    And,

    /// <summary>
    /// The territory code for Netherlands Antilles.
    /// </summary>
    [EnumMember(Value = "ANT")]
    Ant,

    /// <summary>
    /// The territory code for United Arab Emirates.
    /// </summary>
    [EnumMember(Value = "ARE")]
    Are,

    /// <summary>
    /// The territory code for Argentina.
    /// </summary>
    [EnumMember(Value = "ARG")]
    Arg,

    /// <summary>
    /// The territory code for Armenia.
    /// </summary>
    [EnumMember(Value = "ARM")]
    Arm,

    /// <summary>
    /// The territory code for American Samoa.
    /// </summary>
    [EnumMember(Value = "ASM")]
    Asm,

    /// <summary>
    /// The territory code for Antigua and Barbuda.
    /// </summary>
    [EnumMember(Value = "ATG")]
    Atg,

    /// <summary>
    /// The territory code for Australia.
    /// </summary>
    [EnumMember(Value = "AUS")]
    Aus,

    /// <summary>
    /// The territory code for Austria.
    /// </summary>
    [EnumMember(Value = "AUT")]
    Aut,

    /// <summary>
    /// The territory code for Azerbaijan.
    /// </summary>
    [EnumMember(Value = "AZE")]
    Aze,

    /// <summary>
    /// The territory code for Burundi.
    /// </summary>
    [EnumMember(Value = "BDI")]
    Bdi,

    /// <summary>
    /// The territory code for Belgium.
    /// </summary>
    [EnumMember(Value = "BEL")]
    Bel,

    /// <summary>
    /// The territory code for Benin.
    /// </summary>
    [EnumMember(Value = "BEN")]
    Ben,

    /// <summary>
    /// The territory code for Bonaire, Sint Eustatius and Saba.
    /// </summary>
    [EnumMember(Value = "BES")]
    Bes,

    /// <summary>
    /// The territory code for Burkina Faso.
    /// </summary>
    [EnumMember(Value = "BFA")]
    Bfa,

    /// <summary>
    /// The territory code for Bangladesh.
    /// </summary>
    [EnumMember(Value = "BGD")]
    Bgd,

    /// <summary>
    /// The territory code for Bulgaria.
    /// </summary>
    [EnumMember(Value = "BGR")]
    Bgr,

    /// <summary>
    /// The territory code for Bahrain.
    /// </summary>
    [EnumMember(Value = "BHR")]
    Bhr,

    /// <summary>
    /// The territory code for Bahamas.
    /// </summary>
    [EnumMember(Value = "BHS")]
    Bhs,

    /// <summary>
    /// The territory code for Bosnia and Herzegovina.
    /// </summary>
    [EnumMember(Value = "BIH")]
    Bih,

    /// <summary>
    /// The territory code for Belarus.
    /// </summary>
    [EnumMember(Value = "BLR")]
    Blr,

    /// <summary>
    /// The territory code for Belize.
    /// </summary>
    [EnumMember(Value = "BLZ")]
    Blz,

    /// <summary>
    /// The territory code for Bermuda.
    /// </summary>
    [EnumMember(Value = "BMU")]
    Bmu,

    /// <summary>
    /// The territory code for Bolivia.
    /// </summary>
    [EnumMember(Value = "BOL")]
    Bol,

    /// <summary>
    /// The territory code for Brazil.
    /// </summary>
    [EnumMember(Value = "BRA")]
    Bra,

    /// <summary>
    /// The territory code for Barbados.
    /// </summary>
    [EnumMember(Value = "BRB")]
    Brb,

    /// <summary>
    /// The territory code for Brunei Darussalam.
    /// </summary>
    [EnumMember(Value = "BRN")]
    Brn,

    /// <summary>
    /// The territory code for Bhutan.
    /// </summary>
    [EnumMember(Value = "BTN")]
    Btn,

    /// <summary>
    /// The territory code for Botswana.
    /// </summary>
    [EnumMember(Value = "BWA")]
    Bwa,

    /// <summary>
    /// The territory code for Central African Republic.
    /// </summary>
    [EnumMember(Value = "CAF")]
    Caf,

    /// <summary>
    /// The territory code for Canada.
    /// </summary>
    [EnumMember(Value = "CAN")]
    Can,

    /// <summary>
    /// The territory code for Switzerland.
    /// </summary>
    [EnumMember(Value = "CHE")]
    Che,

    /// <summary>
    /// The territory code for Chile.
    /// </summary>
    [EnumMember(Value = "CHL")]
    Chl,

    /// <summary>
    /// The territory code for China.
    /// </summary>
    [EnumMember(Value = "CHN")]
    Chn,

    /// <summary>
    /// The territory code for Côte d'Ivoire.
    /// </summary>
    [EnumMember(Value = "CIV")]
    Civ,

    /// <summary>
    /// The territory code for Cameroon.
    /// </summary>
    [EnumMember(Value = "CMR")]
    Cmr,

    /// <summary>
    /// The territory code for Congo, The Democratic Republic of the.
    /// </summary>
    [EnumMember(Value = "COD")]
    Cod,

    /// <summary>
    /// The territory code for Congo.
    /// </summary>
    [EnumMember(Value = "COG")]
    Cog,

    /// <summary>
    /// The territory code for Cook Islands.
    /// </summary>
    [EnumMember(Value = "COK")]
    Cok,

    /// <summary>
    /// The territory code for Colombia.
    /// </summary>
    [EnumMember(Value = "COL")]
    Col,

    /// <summary>
    /// The territory code for Comoros.
    /// </summary>
    [EnumMember(Value = "COM")]
    Com,

    /// <summary>
    /// The territory code for Cabo Verde.
    /// </summary>
    [EnumMember(Value = "CPV")]
    Cpv,

    /// <summary>
    /// The territory code for Costa Rica.
    /// </summary>
    [EnumMember(Value = "CRI")]
    Cri,

    /// <summary>
    /// The territory code for Cuba.
    /// </summary>
    [EnumMember(Value = "CUB")]
    Cub,

    /// <summary>
    /// The territory code for Curaçao.
    /// </summary>
    [EnumMember(Value = "CUW")]
    Cuw,

    /// <summary>
    /// The territory code for Christmas Island.
    /// </summary>
    [EnumMember(Value = "CXR")]
    Cxr,

    /// <summary>
    /// The territory code for Cayman Islands.
    /// </summary>
    [EnumMember(Value = "CYM")]
    Cym,

    /// <summary>
    /// The territory code for Cyprus.
    /// </summary>
    [EnumMember(Value = "CYP")]
    Cyp,

    /// <summary>
    /// The territory code for Czechia.
    /// </summary>
    [EnumMember(Value = "CZE")]
    Cze,

    /// <summary>
    /// The territory code for Germany.
    /// </summary>
    [EnumMember(Value = "DEU")]
    Deu,

    /// <summary>
    /// The territory code for Djibouti.
    /// </summary>
    [EnumMember(Value = "DJI")]
    Dji,

    /// <summary>
    /// The territory code for Dominica.
    /// </summary>
    [EnumMember(Value = "DMA")]
    Dma,

    /// <summary>
    /// The territory code for Denmark.
    /// </summary>
    [EnumMember(Value = "DNK")]
    Dnk,

    /// <summary>
    /// The territory code for Dominican Republic.
    /// </summary>
    [EnumMember(Value = "DOM")]
    Dom,

    /// <summary>
    /// The territory code for Algeria.
    /// </summary>
    [EnumMember(Value = "DZA")]
    Dza,

    /// <summary>
    /// The territory code for Ecuador.
    /// </summary>
    [EnumMember(Value = "ECU")]
    Ecu,

    /// <summary>
    /// The territory code for Egypt.
    /// </summary>
    [EnumMember(Value = "EGY")]
    Egy,

    /// <summary>
    /// The territory code for Eritrea.
    /// </summary>
    [EnumMember(Value = "ERI")]
    Eri,

    /// <summary>
    /// The territory code for Spain.
    /// </summary>
    [EnumMember(Value = "ESP")]
    Esp,

    /// <summary>
    /// The territory code for Estonia.
    /// </summary>
    [EnumMember(Value = "EST")]
    Est,

    /// <summary>
    /// The territory code for Ethiopia.
    /// </summary>
    [EnumMember(Value = "ETH")]
    Eth,

    /// <summary>
    /// The territory code for Finland.
    /// </summary>
    [EnumMember(Value = "FIN")]
    Fin,

    /// <summary>
    /// The territory code for Fiji.
    /// </summary>
    [EnumMember(Value = "FJI")]
    Fji,

    /// <summary>
    /// The territory code for Falkland Islands (Malvinas).
    /// </summary>
    [EnumMember(Value = "FLK")]
    Flk,

    /// <summary>
    /// The territory code for France.
    /// </summary>
    [EnumMember(Value = "FRA")]
    Fra,

    /// <summary>
    /// The territory code for Faroe Islands.
    /// </summary>
    [EnumMember(Value = "FRO")]
    Fro,

    /// <summary>
    /// The territory code for Micronesia, Federated States of.
    /// </summary>
    [EnumMember(Value = "FSM")]
    Fsm,

    /// <summary>
    /// The territory code for Gabon.
    /// </summary>
    [EnumMember(Value = "GAB")]
    Gab,

    /// <summary>
    /// The territory code for United Kingdom.
    /// </summary>
    [EnumMember(Value = "GBR")]
    Gbr,

    /// <summary>
    /// The territory code for Georgia.
    /// </summary>
    [EnumMember(Value = "GEO")]
    Geo,

    /// <summary>
    /// The territory code for Guernsey.
    /// </summary>
    [EnumMember(Value = "GGY")]
    Ggy,

    /// <summary>
    /// The territory code for Ghana.
    /// </summary>
    [EnumMember(Value = "GHA")]
    Gha,

    /// <summary>
    /// The territory code for Gibraltar.
    /// </summary>
    [EnumMember(Value = "GIB")]
    Gib,

    /// <summary>
    /// The territory code for Guinea.
    /// </summary>
    [EnumMember(Value = "GIN")]
    Gin,

    /// <summary>
    /// The territory code for Guadeloupe.
    /// </summary>
    [EnumMember(Value = "GLP")]
    Glp,

    /// <summary>
    /// The territory code for Gambia.
    /// </summary>
    [EnumMember(Value = "GMB")]
    Gmb,

    /// <summary>
    /// The territory code for Guinea-Bissau.
    /// </summary>
    [EnumMember(Value = "GNB")]
    Gnb,

    /// <summary>
    /// The territory code for Equatorial Guinea.
    /// </summary>
    [EnumMember(Value = "GNQ")]
    Gnq,

    /// <summary>
    /// The territory code for Greece.
    /// </summary>
    [EnumMember(Value = "GRC")]
    Grc,

    /// <summary>
    /// The territory code for Grenada.
    /// </summary>
    [EnumMember(Value = "GRD")]
    Grd,

    /// <summary>
    /// The territory code for Greenland.
    /// </summary>
    [EnumMember(Value = "GRL")]
    Grl,

    /// <summary>
    /// The territory code for Guatemala.
    /// </summary>
    [EnumMember(Value = "GTM")]
    Gtm,

    /// <summary>
    /// The territory code for French Guiana.
    /// </summary>
    [EnumMember(Value = "GUF")]
    Guf,

    /// <summary>
    /// The territory code for Guam.
    /// </summary>
    [EnumMember(Value = "GUM")]
    Gum,

    /// <summary>
    /// The territory code for Guyana.
    /// </summary>
    [EnumMember(Value = "GUY")]
    Guy,

    /// <summary>
    /// The territory code for Hong Kong.
    /// </summary>
    [EnumMember(Value = "HKG")]
    Hkg,

    /// <summary>
    /// The territory code for Honduras.
    /// </summary>
    [EnumMember(Value = "HND")]
    Hnd,

    /// <summary>
    /// The territory code for Croatia.
    /// </summary>
    [EnumMember(Value = "HRV")]
    Hrv,

    /// <summary>
    /// The territory code for Haiti.
    /// </summary>
    [EnumMember(Value = "HTI")]
    Hti,

    /// <summary>
    /// The territory code for Hungary.
    /// </summary>
    [EnumMember(Value = "HUN")]
    Hun,

    /// <summary>
    /// The territory code for Indonesia.
    /// </summary>
    [EnumMember(Value = "IDN")]
    Idn,

    /// <summary>
    /// The territory code for Isle of Man.
    /// </summary>
    [EnumMember(Value = "IMN")]
    Imn,

    /// <summary>
    /// The territory code for India.
    /// </summary>
    [EnumMember(Value = "IND")]
    Ind,

    /// <summary>
    /// The territory code for Ireland.
    /// </summary>
    [EnumMember(Value = "IRL")]
    Irl,

    /// <summary>
    /// The territory code for Iraq.
    /// </summary>
    [EnumMember(Value = "IRQ")]
    Irq,

    /// <summary>
    /// The territory code for Iceland.
    /// </summary>
    [EnumMember(Value = "ISL")]
    Isl,

    /// <summary>
    /// The territory code for Israel.
    /// </summary>
    [EnumMember(Value = "ISR")]
    Isr,

    /// <summary>
    /// The territory code for Italy.
    /// </summary>
    [EnumMember(Value = "ITA")]
    Ita,

    /// <summary>
    /// The territory code for Jamaica.
    /// </summary>
    [EnumMember(Value = "JAM")]
    Jam,

    /// <summary>
    /// The territory code for Jersey.
    /// </summary>
    [EnumMember(Value = "JEY")]
    Jey,

    /// <summary>
    /// The territory code for Jordan.
    /// </summary>
    [EnumMember(Value = "JOR")]
    Jor,

    /// <summary>
    /// The territory code for Japan.
    /// </summary>
    [EnumMember(Value = "JPN")]
    Jpn,

    /// <summary>
    /// The territory code for Kazakhstan.
    /// </summary>
    [EnumMember(Value = "KAZ")]
    Kaz,

    /// <summary>
    /// The territory code for Kenya.
    /// </summary>
    [EnumMember(Value = "KEN")]
    Ken,

    /// <summary>
    /// The territory code for Kyrgyzstan.
    /// </summary>
    [EnumMember(Value = "KGZ")]
    Kgz,

    /// <summary>
    /// The territory code for Cambodia.
    /// </summary>
    [EnumMember(Value = "KHM")]
    Khm,

    /// <summary>
    /// The territory code for Kiribati.
    /// </summary>
    [EnumMember(Value = "KIR")]
    Kir,

    /// <summary>
    /// The territory code for Saint Kitts and Nevis.
    /// </summary>
    [EnumMember(Value = "KNA")]
    Kna,

    /// <summary>
    /// The territory code for South Korea.
    /// </summary>
    [EnumMember(Value = "KOR")]
    Kor,

    /// <summary>
    /// The territory code for Kuwait.
    /// </summary>
    [EnumMember(Value = "KWT")]
    Kwt,

    /// <summary>
    /// The territory code for Laos.
    /// </summary>
    [EnumMember(Value = "LAO")]
    Lao,

    /// <summary>
    /// The territory code for Lebanon.
    /// </summary>
    [EnumMember(Value = "LBN")]
    Lbn,

    /// <summary>
    /// The territory code for Liberia.
    /// </summary>
    [EnumMember(Value = "LBR")]
    Lbr,

    /// <summary>
    /// The territory code for Libya.
    /// </summary>
    [EnumMember(Value = "LBY")]
    Lby,

    /// <summary>
    /// The territory code for Saint Lucia.
    /// </summary>
    [EnumMember(Value = "LCA")]
    Lca,

    /// <summary>
    /// The territory code for Liechtenstein.
    /// </summary>
    [EnumMember(Value = "LIE")]
    Lie,

    /// <summary>
    /// The territory code for Sri Lanka.
    /// </summary>
    [EnumMember(Value = "LKA")]
    Lka,

    /// <summary>
    /// The territory code for Lesotho.
    /// </summary>
    [EnumMember(Value = "LSO")]
    Lso,

    /// <summary>
    /// The territory code for Lithuania.
    /// </summary>
    [EnumMember(Value = "LTU")]
    Ltu,

    /// <summary>
    /// The territory code for Luxembourg.
    /// </summary>
    [EnumMember(Value = "LUX")]
    Lux,

    /// <summary>
    /// The territory code for Latvia.
    /// </summary>
    [EnumMember(Value = "LVA")]
    Lva,

    /// <summary>
    /// The territory code for Macao.
    /// </summary>
    [EnumMember(Value = "MAC")]
    Mac,

    /// <summary>
    /// The territory code for Morocco.
    /// </summary>
    [EnumMember(Value = "MAR")]
    Mar,

    /// <summary>
    /// The territory code for Monaco.
    /// </summary>
    [EnumMember(Value = "MCO")]
    Mco,

    /// <summary>
    /// The territory code for Moldova.
    /// </summary>
    [EnumMember(Value = "MDA")]
    Mda,

    /// <summary>
    /// The territory code for Madagascar.
    /// </summary>
    [EnumMember(Value = "MDG")]
    Mdg,

    /// <summary>
    /// The territory code for Maldives.
    /// </summary>
    [EnumMember(Value = "MDV")]
    Mdv,

    /// <summary>
    /// The territory code for Mexico.
    /// </summary>
    [EnumMember(Value = "MEX")]
    Mex,

    /// <summary>
    /// The territory code for Marshall Islands.
    /// </summary>
    [EnumMember(Value = "MHL")]
    Mhl,

    /// <summary>
    /// The territory code for North Macedonia.
    /// </summary>
    [EnumMember(Value = "MKD")]
    Mkd,

    /// <summary>
    /// The territory code for Mali.
    /// </summary>
    [EnumMember(Value = "MLI")]
    Mli,

    /// <summary>
    /// The territory code for Malta.
    /// </summary>
    [EnumMember(Value = "MLT")]
    Mlt,

    /// <summary>
    /// The territory code for Myanmar.
    /// </summary>
    [EnumMember(Value = "MMR")]
    Mmr,

    /// <summary>
    /// The territory code for Montenegro.
    /// </summary>
    [EnumMember(Value = "MNE")]
    Mne,

    /// <summary>
    /// The territory code for Mongolia.
    /// </summary>
    [EnumMember(Value = "MNG")]
    Mng,

    /// <summary>
    /// The territory code for Northern Mariana Islands.
    /// </summary>
    [EnumMember(Value = "MNP")]
    Mnp,

    /// <summary>
    /// The territory code for Mozambique.
    /// </summary>
    [EnumMember(Value = "MOZ")]
    Moz,

    /// <summary>
    /// The territory code for Mauritania.
    /// </summary>
    [EnumMember(Value = "MRT")]
    Mrt,

    /// <summary>
    /// The territory code for Montserrat.
    /// </summary>
    [EnumMember(Value = "MSR")]
    Msr,

    /// <summary>
    /// The territory code for Martinique.
    /// </summary>
    [EnumMember(Value = "MTQ")]
    Mtq,

    /// <summary>
    /// The territory code for Mauritius.
    /// </summary>
    [EnumMember(Value = "MUS")]
    Mus,

    /// <summary>
    /// The territory code for Malawi.
    /// </summary>
    [EnumMember(Value = "MWI")]
    Mwi,

    /// <summary>
    /// The territory code for Malaysia.
    /// </summary>
    [EnumMember(Value = "MYS")]
    Mys,

    /// <summary>
    /// The territory code for Mayotte.
    /// </summary>
    [EnumMember(Value = "MYT")]
    Myt,

    /// <summary>
    /// The territory code for Namibia.
    /// </summary>
    [EnumMember(Value = "NAM")]
    Nam,

    /// <summary>
    /// The territory code for New Caledonia.
    /// </summary>
    [EnumMember(Value = "NCL")]
    Ncl,

    /// <summary>
    /// The territory code for Niger.
    /// </summary>
    [EnumMember(Value = "NER")]
    Ner,

    /// <summary>
    /// The territory code for Norfolk Island.
    /// </summary>
    [EnumMember(Value = "NFK")]
    Nfk,

    /// <summary>
    /// The territory code for Nigeria.
    /// </summary>
    [EnumMember(Value = "NGA")]
    Nga,

    /// <summary>
    /// The territory code for Nicaragua.
    /// </summary>
    [EnumMember(Value = "NIC")]
    Nic,

    /// <summary>
    /// The territory code for Niue.
    /// </summary>
    [EnumMember(Value = "NIU")]
    Niu,

    /// <summary>
    /// The territory code for Netherlands.
    /// </summary>
    [EnumMember(Value = "NLD")]
    Nld,

    /// <summary>
    /// The territory code for Norway.
    /// </summary>
    [EnumMember(Value = "NOR")]
    Nor,

    /// <summary>
    /// The territory code for Nepal.
    /// </summary>
    [EnumMember(Value = "NPL")]
    Npl,

    /// <summary>
    /// The territory code for Nauru.
    /// </summary>
    [EnumMember(Value = "NRU")]
    Nru,

    /// <summary>
    /// The territory code for New Zealand.
    /// </summary>
    [EnumMember(Value = "NZL")]
    Nzl,

    /// <summary>
    /// The territory code for Oman.
    /// </summary>
    [EnumMember(Value = "OMN")]
    Omn,

    /// <summary>
    /// The territory code for Pakistan.
    /// </summary>
    [EnumMember(Value = "PAK")]
    Pak,

    /// <summary>
    /// The territory code for Panama.
    /// </summary>
    [EnumMember(Value = "PAN")]
    Pan,

    /// <summary>
    /// The territory code for Peru.
    /// </summary>
    [EnumMember(Value = "PER")]
    Per,

    /// <summary>
    /// The territory code for Philippines.
    /// </summary>
    [EnumMember(Value = "PHL")]
    Phl,

    /// <summary>
    /// The territory code for Palau.
    /// </summary>
    [EnumMember(Value = "PLW")]
    Plw,

    /// <summary>
    /// The territory code for Papua New Guinea.
    /// </summary>
    [EnumMember(Value = "PNG")]
    Png,

    /// <summary>
    /// The territory code for Poland.
    /// </summary>
    [EnumMember(Value = "POL")]
    Pol,

    /// <summary>
    /// The territory code for Puerto Rico.
    /// </summary>
    [EnumMember(Value = "PRI")]
    Pri,

    /// <summary>
    /// The territory code for Portugal.
    /// </summary>
    [EnumMember(Value = "PRT")]
    Prt,

    /// <summary>
    /// The territory code for Paraguay.
    /// </summary>
    [EnumMember(Value = "PRY")]
    Pry,

    /// <summary>
    /// The territory code for Palestine, State of.
    /// </summary>
    [EnumMember(Value = "PSE")]
    Pse,

    /// <summary>
    /// The territory code for French Polynesia.
    /// </summary>
    [EnumMember(Value = "PYF")]
    Pyf,

    /// <summary>
    /// The territory code for Qatar.
    /// </summary>
    [EnumMember(Value = "QAT")]
    Qat,

    /// <summary>
    /// The territory code for Réunion.
    /// </summary>
    [EnumMember(Value = "REU")]
    Reu,

    /// <summary>
    /// The territory code for Romania.
    /// </summary>
    [EnumMember(Value = "ROU")]
    Rou,

    /// <summary>
    /// The territory code for Russian Federation.
    /// </summary>
    [EnumMember(Value = "RUS")]
    Rus,

    /// <summary>
    /// The territory code for Rwanda.
    /// </summary>
    [EnumMember(Value = "RWA")]
    Rwa,

    /// <summary>
    /// The territory code for Saudi Arabia.
    /// </summary>
    [EnumMember(Value = "SAU")]
    Sau,

    /// <summary>
    /// The territory code for Senegal.
    /// </summary>
    [EnumMember(Value = "SEN")]
    Sen,

    /// <summary>
    /// The territory code for Singapore.
    /// </summary>
    [EnumMember(Value = "SGP")]
    Sgp,

    /// <summary>
    /// The territory code for Saint Helena, Ascension and Tristan da Cunha.
    /// </summary>
    [EnumMember(Value = "SHN")]
    Shn,

    /// <summary>
    /// The territory code for Solomon Islands.
    /// </summary>
    [EnumMember(Value = "SLB")]
    Slb,

    /// <summary>
    /// The territory code for Sierra Leone.
    /// </summary>
    [EnumMember(Value = "SLE")]
    Sle,

    /// <summary>
    /// The territory code for El Salvador.
    /// </summary>
    [EnumMember(Value = "SLV")]
    Slv,

    /// <summary>
    /// The territory code for San Marino.
    /// </summary>
    [EnumMember(Value = "SMR")]
    Smr,

    /// <summary>
    /// The territory code for Somalia.
    /// </summary>
    [EnumMember(Value = "SOM")]
    Som,

    /// <summary>
    /// The territory code for Saint Pierre and Miquelon.
    /// </summary>
    [EnumMember(Value = "SPM")]
    Spm,

    /// <summary>
    /// The territory code for Serbia.
    /// </summary>
    [EnumMember(Value = "SRB")]
    Srb,

    /// <summary>
    /// The territory code for South Sudan.
    /// </summary>
    [EnumMember(Value = "SSD")]
    Ssd,

    /// <summary>
    /// The territory code for Sao Tome and Principe.
    /// </summary>
    [EnumMember(Value = "STP")]
    Stp,

    /// <summary>
    /// The territory code for Suriname.
    /// </summary>
    [EnumMember(Value = "SUR")]
    Sur,

    /// <summary>
    /// The territory code for Slovakia.
    /// </summary>
    [EnumMember(Value = "SVK")]
    Svk,

    /// <summary>
    /// The territory code for Slovenia.
    /// </summary>
    [EnumMember(Value = "SVN")]
    Svn,

    /// <summary>
    /// The territory code for Sweden.
    /// </summary>
    [EnumMember(Value = "SWE")]
    Swe,

    /// <summary>
    /// The territory code for Eswatini.
    /// </summary>
    [EnumMember(Value = "SWZ")]
    Swz,

    /// <summary>
    /// The territory code for Sint Maarten (Dutch part).
    /// </summary>
    [EnumMember(Value = "SXM")]
    Sxm,

    /// <summary>
    /// The territory code for Seychelles.
    /// </summary>
    [EnumMember(Value = "SYC")]
    Syc,

    /// <summary>
    /// The territory code for Turks and Caicos Islands.
    /// </summary>
    [EnumMember(Value = "TCA")]
    Tca,

    /// <summary>
    /// The territory code for Chad.
    /// </summary>
    [EnumMember(Value = "TCD")]
    Tcd,

    /// <summary>
    /// The territory code for Togo.
    /// </summary>
    [EnumMember(Value = "TGO")]
    Tgo,

    /// <summary>
    /// The territory code for Thailand.
    /// </summary>
    [EnumMember(Value = "THA")]
    Tha,

    /// <summary>
    /// The territory code for Tajikistan.
    /// </summary>
    [EnumMember(Value = "TJK")]
    Tjk,

    /// <summary>
    /// The territory code for Turkmenistan.
    /// </summary>
    [EnumMember(Value = "TKM")]
    Tkm,

    /// <summary>
    /// The territory code for Timor-Leste.
    /// </summary>
    [EnumMember(Value = "TLS")]
    Tls,

    /// <summary>
    /// The territory code for Tonga.
    /// </summary>
    [EnumMember(Value = "TON")]
    Ton,

    /// <summary>
    /// The territory code for Trinidad and Tobago.
    /// </summary>
    [EnumMember(Value = "TTO")]
    Tto,

    /// <summary>
    /// The territory code for Tunisia.
    /// </summary>
    [EnumMember(Value = "TUN")]
    Tun,

    /// <summary>
    /// The territory code for Türkiye.
    /// </summary>
    [EnumMember(Value = "TUR")]
    Tur,

    /// <summary>
    /// The territory code for Tuvalu.
    /// </summary>
    [EnumMember(Value = "TUV")]
    Tuv,

    /// <summary>
    /// The territory code for Taiwan.
    /// </summary>
    [EnumMember(Value = "TWN")]
    Twn,

    /// <summary>
    /// The territory code for Tanzania.
    /// </summary>
    [EnumMember(Value = "TZA")]
    Tza,

    /// <summary>
    /// The territory code for Uganda.
    /// </summary>
    [EnumMember(Value = "UGA")]
    Uga,

    /// <summary>
    /// The territory code for Ukraine.
    /// </summary>
    [EnumMember(Value = "UKR")]
    Ukr,

    /// <summary>
    /// The territory code for United States Minor Outlying Islands.
    /// </summary>
    [EnumMember(Value = "UMI")]
    Umi,

    /// <summary>
    /// The territory code for Uruguay.
    /// </summary>
    [EnumMember(Value = "URY")]
    Ury,

    /// <summary>
    /// The territory code for United States.
    /// </summary>
    [EnumMember(Value = "USA")]
    Usa,

    /// <summary>
    /// The territory code for Uzbekistan.
    /// </summary>
    [EnumMember(Value = "UZB")]
    Uzb,

    /// <summary>
    /// The territory code for Holy See (Vatican City State).
    /// </summary>
    [EnumMember(Value = "VAT")]
    Vat,

    /// <summary>
    /// The territory code for Saint Vincent and the Grenadines.
    /// </summary>
    [EnumMember(Value = "VCT")]
    Vct,

    /// <summary>
    /// The territory code for Venezuela.
    /// </summary>
    [EnumMember(Value = "VEN")]
    Ven,

    /// <summary>
    /// The territory code for Virgin Islands, British.
    /// </summary>
    [EnumMember(Value = "VGB")]
    Vgb,

    /// <summary>
    /// The territory code for Virgin Islands, U.S..
    /// </summary>
    [EnumMember(Value = "VIR")]
    Vir,

    /// <summary>
    /// The territory code for Vietnam.
    /// </summary>
    [EnumMember(Value = "VNM")]
    Vnm,

    /// <summary>
    /// The territory code for Vanuatu.
    /// </summary>
    [EnumMember(Value = "VUT")]
    Vut,

    /// <summary>
    /// The territory code for Wallis and Futuna.
    /// </summary>
    [EnumMember(Value = "WLF")]
    Wlf,

    /// <summary>
    /// The territory code for Samoa.
    /// </summary>
    [EnumMember(Value = "WSM")]
    Wsm,

    /// <summary>
    /// The territory code for Kosovo.
    /// </summary>
    [EnumMember(Value = "XKS")]
    Xks,

    /// <summary>
    /// The territory code for Yemen.
    /// </summary>
    [EnumMember(Value = "YEM")]
    Yem,

    /// <summary>
    /// The territory code for South Africa.
    /// </summary>
    [EnumMember(Value = "ZAF")]
    Zaf,

    /// <summary>
    /// The territory code for Zambia.
    /// </summary>
    [EnumMember(Value = "ZMB")]
    Zmb,

    /// <summary>
    /// The territory code for Zimbabwe.
    /// </summary>
    [EnumMember(Value = "ZWE")]
    Zwe
}
