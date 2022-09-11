<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%: Html.Telerik().Menu()
        .Name("Menu")
        .Effects(fx => fx.Expand().Opacity().OpenDuration(200).CloseDuration(200)
        )
        .Items(menu =>
        {
            menu.Add().Text("Add Employee").Action("Insert", "Employee", new { area = "Personnel" });

            menu.Add().Text("Employees List").Action("Index", "Employee", new { area = "Personnel" });

            menu.Add().Text("Indexes")
                .Items(item =>
                {
                    item.Add().Text("General")
                        .Items(child =>
                        {
                            child.Add().Text("Country").Action("Index", "Country", new { area = "Personnel" });
                            child.Add().Text("Nationality").Action("Index", "Nationality", new { area = "Personnel" });
                            child.Add().Text("Bool Condition").Action("Index", "BoolCondition", new { area = "Personnel" });
                        });
                    
                    item.Add().Text("Employee Info.")
                        .Items(child =>
                        {
                            child.Add().Text("Blood Group").Action("Index", "BloodType", new { area = "Personnel" });
                            child.Add().Text("Gender").Action("Index", "Gender", new { area = "Personnel" });
                            child.Add().Text("Marital Status").Action("Index", "MaritalStatus", new { area = "Personnel" });
                            child.Add().Text("Military Status").Action("Index", "MilitaryStatus", new { area = "Personnel" });
                            child.Add().Text("Race").Action("Index", "Race", new { area = "Personnel" });
                            child.Add().Text("Religion").Action("Index", "Religion", new { area = "Personnel" });
                        });

                    item.Add().Text("Identifications")
                        .Items(child =>
                        {
                            child.Add().Text("Driving License Type").Action("Index", "DrivingLicenseType", new { area = "Personnel" });
                            child.Add().Text("Residency Type").Action("Index", "ResidencyType", new { area = "Personnel" });
                        });

                    item.Add().Text("Qualifications")
                        .Items(child =>
                        {
                            child.Add().Text("Certification Type").Action("Index", "CertificationType", new { area = "Personnel" });
                            child.Add().Text("Level").Action("Index", "Level", new { area = "Personnel" });
                            child.Add().Text("Major").Action("Index", "Major", new { area = "Personnel" });
                            child.Add().Text("Academic Degree").Action("Index", "MajorType", new { area = "Personnel" });
                            child.Add().Text("Skill Type").Action("Index", "SkillType", new { area = "Personnel" });
                            child.Add().Text("Score Type").Action("Index", "ScoreType", new { area = "Personnel" });
                        });

                    item.Add().Text("Convictions")
                    .Items(child => child.Add().Text("Conviction Rule").Action("Index", "ConvictionRule", new { area = "Personnel" }));
                    });
        })
%>