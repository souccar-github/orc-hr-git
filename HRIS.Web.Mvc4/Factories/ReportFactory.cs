using  Project.Web.Mvc4.Models.Navigation;
using Souccar.Domain.Reporting;
using Souccar.Domain.Extensions;
using Souccar.Infrastructure.Core;
using Souccar.Infrastructure.Extenstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  Project.Web.Mvc4.Extensions;

namespace Project.Web.Mvc4.Factories
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public class ReportFactory
    {
        public static IList<Report> Create(string moduleName)
        {
            return ServiceFactory.ORMService.All<ReportDefinition>()
                .Where(x => x.ModuleName ==  moduleName)
                .Select(x => new Report()
                {
                    Description = x.Description,
                    Id = x.Id,
                    ReportId = x.Id.ToString(),
                    SecurityId = x.Id.ToString(),
                    Title = ServiceFactory.LocalizationService.GetResource(x.FileName) ?? x.FileName,
                    FileName = x.FileName,
                    Controller = "ReportingPreview",
                    Action = "Index"
                }).ToList();
        }
        //public static void CreateDefaultReports()
        //{
        //    // Objective
        //    CreateReport("الهدف المعرف على مستوى الموقع الوظيفي ", "01_7365_.frx_01.frx", ModuleName.Objectives);
        //    CreateReport("عرض الاهداف المرتبطة بالهدف الإستراتيجي", "02_7017_.frx_02.frx", ModuleName.Objectives);
        //    CreateReport("عرض الاهداف حسب المالك", "03_18344_.frx_03.frx", ModuleName.Objectives);
        //    CreateReport("عرض الهيكل التنظيمي حسب الهدف", "04_6997_.frx_04.frx", ModuleName.Objectives);

        //    // Training
        //    CreateReport("الخطة التدريبية التي تغطي الاحتياجات المطلوبة والدورات المقترحة لسدها", "05_10578_.frx_05.frx", ModuleName.Training);
        //    CreateReport("تقرير احصائي لنسبة حضور دورة معينة", "06_4565_.frx_06.frx", ModuleName.Training);
        //    CreateReport("تقرير احصائي لنسبة مشاركة موظف معين في الدورات نسبة للدورات المقامة في فترة معينة", "07_3572_.frx_07.frx", ModuleName.Training);
        //    CreateReport("تقرير عن تاريخ الموظف التدريبي خلال فترة معينة بالدورات المستفيد منها ومعلوماتها وكلفها", "08_14969_.frx_08.frx", ModuleName.Training);
        //    CreateReport("تقرير عن تقييم التدريب من قبل المتدربين", "09_14225_.frx_09.frx", ModuleName.Training);
        //    CreateReport("تقرير عن تقييم متدربين في دورة تدريبية معينة", "10_14405_.frx_10.frx", ModuleName.Training);
        //    CreateReport("تقرير عن تكاليف دورة معينة", "11_11407_.frx_11.frx", ModuleName.Training);
        //    CreateReport("تقرير عن عدد الموظفين الذين لديهم حاجة تدريبية معينة", "12_11195_.frx_12.frx", ModuleName.Training);
        //    CreateReport("تقرير عن عدد وأسماء المرشحين لدورة تدريبية معينة مع معلوماتهم الوظيفية", "13_16597_.frx_13.frx", ModuleName.Training);
        //    CreateReport("تقرير عن نسبة الاحتياجات التدريبية في فروع و مديريات المصرف", "14_4230_.frx_14.frx", ModuleName.Training);
        //    CreateReport("تقرير مفصل أو جزئي عن معلومات دورة إما بحالة التخطيط أو بحالة التنفيذ", "15_16103_.frx_15.frx", ModuleName.Training);
        //    CreateReport("عدد وأسماء المتدربين في دورة معينة مع معلوماتهم الوظيفية", "16_16280_.frx_16.frx", ModuleName.Training);
        //    CreateReport("عدد ومعلومات الاحتياجات التدريبية في فروع و مديريات المصرف", "17_10339_.frx_17.frx", ModuleName.Training);
        //    CreateReport("معلومات الاحتياجات التدريبية في فروع و مديريات المصرف", "18_10259_.frx_18.frx", ModuleName.Training);
        //    CreateReport("تقرير شجرة المسار الوظيفي", "68_10344_.frx_68.frx", ModuleName.Training);
        //    CreateReport("تقرير عن المسار الوظيفي لمسمى وظيفي معين", "69_10344_.frx_69.frx", ModuleName.Training);
        //    CreateReport("تقرير احصائي عن نسب مقاربة مواصفات مسمى وظيفي معين بالنسبة للموظفين مع عرض أسماء المرشحين بترتيب الأولوية في عملية المطابقة", "67_10344_.frx_67.frx", ModuleName.Training);

        //    // Incentive
        //    CreateReport("تقرير بتصفية الحوافز للموظفين", "19_7688_.frx_19.frx", ModuleName.Incentive);
        //    CreateReport("تقرير تفصيلي عن معلومات تقييم والحوافز المستحقة على خلال مرحلة حوافز معينة", "20_9688_.frx_20.frx", ModuleName.Incentive);
        //    CreateReport("تقرير عن الموظفين الغير مستحقين للحوافز في مرحلة حوافز معينة ", "21_7761_.frx_21.frx", ModuleName.Incentive);
        //    CreateReport("تقرير عن تقييم الموظفين التابعين لعقدة معينة خلال مرحلة معينة", "22_11462_.frx_22.frx", ModuleName.Incentive);
        //    CreateReport("تقرير عن نسبة الموظفين خلال مرحلة تقييم معينة", "23_8003_.frx_23.frx", ModuleName.Incentive);
        //    CreateReport("تقرير تفصيلي عن تقييم موظف خلال مرحلة حوافز  معينة حيث يتم فيه استعراض استمارة التقييم الخاصة بالموظف مع معلوماتها", "70_10344_.frx_70.frx", ModuleName.Incentive);
        //    CreateReport("تقرير عن عدد ونسبة الموظفين الذين تم حرمانهم من الحوافز  خلال مرحلة معينة", "71_10344_.frx_71.frx", ModuleName.Incentive);
        //    CreateReport("تقرير عن عدد ونسبة الموظفين الذين تم شملهم بالحوافز خلال مرحلة حوافز معينة ", "72_10344_.frx_72.frx", ModuleName.Incentive);

        //    // HealthInsurance
        //    CreateReport("تقرير إصابات العمل", "24_8054_.frx_24.frx", ModuleName.HealthInsurance);
        //    CreateReport("تقرير المفوضين من قبل موظف", "25_5314_.frx_25.frx", ModuleName.HealthInsurance);
        //    CreateReport("تقرير الموظفين الغير مفوضين", "26_2161_.frx_26.frx", ModuleName.HealthInsurance);
        //    CreateReport("تقرير إصابات العمل - لموظف", "27_6459_.frx_27.frx", ModuleName.HealthInsurance);

        //    // PayrollSystem
        //    CreateReport("تقرير الاستحقاقات السالبة للموظفين", "28_21737_.frx_28.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير مقبوضات موظف", "29_22602_.frx_29.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير أذونات السفر الخارجية", "30_19652_.frx_30.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير أذونات السفر الداخلية", "31_17929_.frx_31.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير التأمينات الاجتماعية الاستمارة رقم (2)", "32_6423_.frx_32.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير الموظفين الخاضعين لتعويض ثابت معين", "33_14275_.frx_33.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير الموظفين الخاضعين لحسم ثابت معين", "34_11851_.frx_34.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير تحويل الرواتب", "35_6946_.frx_35.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير قروض الموظفين", "36_14549_.frx_36.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير مبالغ التعويضات الممنوحة", "37_9137_.frx_37.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير مبالغ الحسميات المقتطعة", "38_9843_.frx_38.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير الموظفين ضمن عقدة محددة", "96_10344_.frx_96.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير الرواتب الشهرية", "97_10344_.frx_97.frx", ModuleName.PayrollSystem);

        //    // Personnel
        //    CreateReport("تقرير إحصائي لمواليد بتاريخ معين", "39_2880_.frx_39.frx", ModuleName.Personnel);
        //    CreateReport("تقرير عن الحالة العائلية", "40_3157_.frx_40.frx", ModuleName.Personnel);
        //    CreateReport("عدد اللغات الحاصل عليها الموظف", "41_2410_.frx_41.frx", ModuleName.Personnel);
        //    CreateReport("عرض قائمة الموظفين وعدد الاولاد لكل موظف", "42_2668_.frx_42.frx", ModuleName.Personnel);
        //    CreateReport("نسبة الاديان", "43_3037_.frx_43.frx", ModuleName.Personnel);
        //    CreateReport("نسبة الذكور والإناث", "44_3052_.frx_44.frx", ModuleName.Personnel);

        //    // OrganizationChart
        //    CreateReport("الهيكل التنظيمي بحسب المسميات الوظيفية والمواقع الشاغرة", "45_3271_.frx_45.frx", ModuleName.OrganizationChart);
        //    CreateReport("الهيكل التنظيمي بشكل هرمي", "46_2514_.frx_46.frx", ModuleName.OrganizationChart);
        //    CreateReport("الهيكل التنظيمي وربطه بالهيكل الإداري  اي المسميات الوظيفية", "47_2846_.frx_47.frx", ModuleName.OrganizationChart);

        //    // Grade
        //    CreateReport("الفئات حسب المؤهلات العلمية", "48_6632_.frx_48.frx", ModuleName.Grades);
        //    CreateReport("الفئات حسب النظام الداخلي", "49_6403_.frx_49.frx", ModuleName.Grades);
        //    CreateReport("تقرير عن الفئة والمسميات والمواقع الوظيفية المرتبطة بها مع الملاك العددي", "50_9446_.frx_50.frx", ModuleName.Grade);

        //    // JobDescription
        //    CreateReport("الخبرات العلمية لمسمى وظيفي محدد", "51_9379_.frx_51.frx", ModuleName.JobDescription);
        //    CreateReport("الصلاحيات المتاحة لمسمى وظيفي محدد", "52_11854_.frx_52.frx", ModuleName.JobDescription);
        //    CreateReport("القيود الوظيفية لمسمى وظيفي محدد", "53_11896_.frx_53.frx", ModuleName.JobDescription);
        //    CreateReport("الكفاءات الوظيفية لمسمى وظيفي محدد", "54_9252_.frx_54.frx", ModuleName.JobDescription);
        //    CreateReport("اللغات المطلوبة لمسمى وظيفي محدد", "55_12119_.frx_55.frx", ModuleName.JobDescription);
        //    CreateReport("المهارات الوظيفية لمسمى وظيفي محدد", "56_9439_.frx_56.frx", ModuleName.JobDescription);
        //    CreateReport("المؤهلات العلمية لمسمى وظيفي محدد", "57_9561_.frx_57.frx", ModuleName.JobDescription);
        //    CreateReport("الواجبات والمسؤوليات لمسمى وظيفي محدد", "58_9289_.frx_58.frx", ModuleName.JobDescription);
        //    CreateReport("عدد ونسبة المواقع الوظيفية التي لها بطاقة وصف وظيفي محدد", "59_7063_.frx_59.frx", ModuleName.JobDescription);
        //    CreateReport("مواصفات المسمى الوظيفي محدد", "60_8123_.frx_60.frx", ModuleName.JobDescription);
        //    CreateReport("نسبة بطاقات الوصف الوظيفي", "61_6053_.frx_61.frx", ModuleName.JobDescription);
        //    CreateReport("نسبة وعدد المسميات الوظيفية", "62_6084_.frx_62.frx", ModuleName.JobDescription);

        //    // PMS
        //    CreateReport("تقرير يقوم بعرض معلومات الترفيعات عن تاريخ محدد", "63_13061_.frx_63.frx", ModuleName.PMS);
        //    CreateReport("نموذج تقييم العاملين لدى مصرف سورية المركزي حسب الفرع", "64_13868_.frx_64.frx", ModuleName.PMS);
        //    CreateReport("تقرير بنسب نقاط الضعف ونقاط القوة وتجميعها حسب العقدة أو حسب الحاجة التدريبية خلال مرحلة تقييم معينة", "73_10344_.frx_73.frx", ModuleName.PMS);
        //    CreateReport("تقرير بنقاط الضعف ونقاط القوة وتجميعها حسب العقدة أو حسب الحاجة التدريبية", "74_10344_.frx_74.frx", ModuleName.PMS);
        //    CreateReport("تقرير تفصيلي عن تقييم موظف خلال مرحلة تقييم معينة حيث يتم فيه استعراض استمارة التقييم الخاصة بالموظف مع معلوماتها", "75_10344_.frx_75.frx", ModuleName.PMS);
        //    CreateReport("تقرير عن تقييم الموظفين التابعين لعقدة معينة حيث يتم فيه عرض ملخص عن معلومات تقييم الموظفين التابعين للعقدة المختارة خلال مرحلة معينة", "76_10344_.frx_76.frx", ModuleName.PMS);
        //    CreateReport("تقرير عن عدد ونسبة الموظفين الذين تم ترفيعهم خلال مرحلة ترفيع معينة ", "77_10344_.frx_77.frx", ModuleName.PMS);
        //    CreateReport("تقرير عن عدد ونسبة الموظفين الذين لم يتم ترفيعهم خلال مرحلة ترفيع معينة ", "78_10344_.frx_78.frx", ModuleName.PMS);
        //    CreateReport("تقرير عن نسبة الموظفين حسب درجة الكفاءة خلال مرحلة تقييم معينة-تقييم", "79_10344_.frx_79.frx", ModuleName.PMS);
        //    CreateReport("تقرير عن تقييم وترفيع الموظفين التابعين لعقدة معينة حيث يتم عرض ملخص عن معلومات تقييم وترفيع الموظفين التابعين للعقدة المختارة خلال مرحلة ترفيع معينة ", "80_10344_.frx_80.frx", ModuleName.PMS);
        //    CreateReport("تقرير تفصيلي عن معلومات تقييم والترفيع المستحق على أساسه لموظف خلال مرحلة ترفيع معينة", "81_10344_.frx_81.frx", ModuleName.PMS);

        //    // EmployeeRelationServices
        //    CreateReport("واقعات الموظف", "65_19313_.frx_65.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("وقوعات موظف محدد", "66_18162_.frx_66.frx", ModuleName.EmployeeRelationServices);

        //    // Recruitment
        //    CreateReport("تقرير احصائي لنسبة نجاح الامتحان التحريري في مسابقة معينة", "82_10344_.frx_82.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير احصائي لنسبة نجاح الامتحان الشفهي في مسابقة معينة", "83_10344_.frx_83.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير بقوائم أسماء الناجحين النهائية ", "84_10344_.frx_84.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير بمعلومات المتقدمين للامتحان التحريري", "85_10344_.frx_85.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير بمعلومات المقبولين", "86_10344_.frx_86.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير بمعلومات الناجحين بالامتحان التحريري", "87_10344_.frx_87.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير بمعلومات الناجحين بالامتحان الشفهي", "88_10344_.frx_88.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير شامل بمعلومات متقدم والنتائج الحاصل عليها", "89_10344_.frx_89.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير عن عدد الشواغر في المصرف لوظيفة معينة ", "90_10344_.frx_90.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير عن عدد ونسبة المقبولين في مسابقة -اختبار- معينة", "91_10344_.frx_91.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير عن معلومات  اعلان معينة", "92_10344_.frx_92.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير عن معلومات المتقدمين", "93_10344_.frx_93.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير عن معلومات مسابقات التوظيف لمتسابق معين", "94_10344_.frx_94.frx", ModuleName.Recruitment);
        //    CreateReport("تقرير عن نسبة النجاح في مسابقة -اختبار- توظيف معينة", "95_10344_.frx_95.frx", ModuleName.Recruitment);

        //}

        //public static void CreatePSDefaultReports()
        //{
        //    CreateReport("تقرير مقبوضات موظف ", "1001.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير الرواتب الشهرية ", "1002.frx", ModuleName.PayrollSystem);
        //    CreateReport("تقرير احصائيات الدوام بدون تقاص ", "1003.frx", ModuleName.AttendanceSystem);
        //    CreateReport("تقرير احصائيات الدوام تقاص شهري ", "1004.frx", ModuleName.AttendanceSystem);
        //    CreateReport("تقرير احصائيات الدوام تقاص يومي ", "1005.frx", ModuleName.AttendanceSystem);
        //    CreateReport("تقرير الدوام لموظف ", "1006.frx", ModuleName.AttendanceSystem);
        //    CreateReport("تقرير الموظفين حسب نموذج الدوام ", "1007.frx", ModuleName.AttendanceSystem);

        //    CreateReport("تقرير الموظفين حسب بلد الولادة  ", "1008.frx", ModuleName.Personnel);
        //    CreateReport("تقرير العهد للموظف ", "1009.frx", ModuleName.Personnel);
        //    CreateReport("تقرير المعلومات المصرفية للموظفين ", "1010.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين الغير مسجلين بالتأمينات الاجتماعية ", "1011.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين المحكومين ", "1012.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين المستقيلين ", "1013.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين المنتهية خدمتهم ", "1014.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين تحت التجربة ", "1015.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب الجنسية الأخرى ", "1016.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب الجنسية الاساسية ", "1017.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب الحالة الاجتماعية ", "1018.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب الضمان الصحي ", "1019.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب بيانات اللغة ", "1020.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب تاريخ  انتهاء الاقامة ", "1021.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب تاريخ  انتهاء جواز السفر  ", "1022.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب تاريخ  انتهاء شهادة القيادة ", "1023.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب حالة خدمة العلم  ", "1024.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب زمرة الدم  ", "1025.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب عدد الاولاد  ", "1026.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب عدد المعالين  ", "1027.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب مجال العمر ", "1028.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين حسب مجال سنوات الخبرة  ", "1029.frx", ModuleName.Personnel);
        //    CreateReport("تقرير الموظفين ذوي الاحتياجات الخاصة ", "1030.frx", ModuleName.Personnel);
        //    CreateReport("تقرير بيانات الدورات التدريبية ", "1031.frx", ModuleName.Personnel);
        //    CreateReport("تقرير بيانات الشريك ", "1032.frx", ModuleName.Personnel);
        //    CreateReport("تقرير بيانات الشهادة ", "1033.frx", ModuleName.Personnel);
        //    CreateReport("تقرير بيانات المستوى التعليمي ", "1034.frx", ModuleName.Personnel);
        //    CreateReport("تقرير توزع الاناث والذكور حسب الاقسام ", "1035.frx", ModuleName.Personnel);
        //    CreateReport("تقرير توزع الديانة حسب الاقسام ", "1036.frx", ModuleName.Personnel);
        //    CreateReport("تقرير عناوين الموظفين  ", "1037.frx", ModuleName.Personnel);
        //    CreateReport("تقرير معلومات التأمينات الاجتماعية للموظفين ", "1038.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب الاناث والذكور ", "1039.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب الاناث والذكور لكامل المؤسسة ", "1040.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب الجنسية الأخرى ضمن المؤسسة ", "1041.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب الجنسية الاساسية ضمن المؤسسة ", "1042.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب الحالة الاجتماعية ضمن المؤسسة ", "1043.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب العجز ضمن المؤسسة ", "1044.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب ديانة الموظفين حسب العقدة ", "1045.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب ديانة الموظفين لكامل المؤسسة ", "1046.frx", ModuleName.Personnel);
        //    CreateReport("تقرير نسب زمرة الدم ضمن المؤسسة  ", "1047.frx", ModuleName.Personnel);
        //    CreateReport("تقرير تعويضات الموظف", "1072.frx", ModuleName.Personnel);

        //    CreateReport("تقرير الموظفين الغير مرتبطين بفئة ", "1048.frx", ModuleName.Grade);
        //    CreateReport("تقرير الموظفين حسب الفئات  ", "1049.frx", ModuleName.Grade);
        //    CreateReport("تقرير تعويضات الفئة  ", "1050.frx", ModuleName.Grade);
        //    CreateReport("تقرير سلم الاجور حسب الفئات ", "1051.frx", ModuleName.Grade);

        //    CreateReport("تقرير المواقع الوظيفية الجديدة  ", "1052.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير المواقع الوظيفية الشاغرة ", "1053.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير المواقع الوظيفية حسب الوصف الوظيفي ", "1054.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير المواقع الوظيفية للموظف ", "1055.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير الموظفين الغير معينين بموقع وظيفي ", "1056.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير تاريخ الموقع الوظيفي ", "1057.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير قائمة الوصف الوظيفي ", "1058.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير كفاءات الوصف الوظيفي ", "1059.frx", ModuleName.JobDescription);
        //    CreateReport("تقرير مطابقة المهارات ", "1060.frx", ModuleName.JobDescription);

        //    CreateReport("تقرير اجازات موظف بين تاريخين ", "1061.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير اجازات موظفي عقدة بين تاريخين ", "1062.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير الموظفين المستقيلين ", "1063.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير الموظفين المنتهية خدمتهم ", "1064.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير تاريخ الموظف ", "1065.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير رصيد اجازات موظف ", "1066.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير رصيد اجازات موظفي عقدة  ", "1067.frx", ModuleName.EmployeeRelationServices);
        //    CreateReport("تقرير عمليات النقل والترقية لموظف ", "1068.frx", ModuleName.EmployeeRelationServices);

        //    CreateReport("تقرير الهيكل التنظيمي حسب الفئات ", "1070.frx", ModuleName.OrganizationChart);
        //    CreateReport("تقرير الهيكل التنظيمي حسب المواقع الوظيفية ", "1071.frx", ModuleName.OrganizationChart);
        //    CreateReport("تقرير تقييم الموظفين ", "1073.frx", ModuleName.PMS);

        //}

        public static void CreateReport(string name, string fileName, string moduleName)
        {
            var report = new ReportDefinition()
            {
                Title = name,
                ModuleName = moduleName,
                CreationDate = DateTime.Now,
                FileName = fileName,

            }.Save();
        }
    }


}
