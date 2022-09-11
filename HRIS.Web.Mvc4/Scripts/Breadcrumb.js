function createBreadcrumb() {
    var result=
            "<div id='breadcrumb'>"+
                 "<span class='title_icon'>"+
                     "<span class='"+window.requestInformation.NavigationInfo.Module.SmallImageClass +"'>"+
                     "</span>"+
                 "</span>"+
                 "<div>"+
                     "<div class='breadcrumb_area'>"+
                         "<a href='" + window.applicationpath + "Module/Welcome/" + window.requestInformation.NavigationInfo.Module.Name + "'>" +
                             window.requestInformation.NavigationInfo.Module.Title+
                         "</a>"+
                     "</div>"
    ;
                    if (window.requestInformation.NavigationInfo.Previous.length!=0) {
                        result += "<div class='arrowbread'></div>";
                    }

    result += "</div>";
              if(window.requestInformation.NavigationInfo.Previous.length!=0){
                  for (var i = 0; i < window.requestInformation.NavigationInfo.Previous.length; i++) {
                      result += "<div>" +
                                  "<div class='breadcrumb_links'>"+
                                      "<a href='#' onclick=\"breadcrumbClick('"+window.requestInformation.NavigationInfo.Previous[i].Name+"')\" >"+
                                           window.requestInformation.NavigationInfo.Previous[i].Title+
                                       "</a>"+
                                 "</div>";
                                 if (i<window.requestInformation.NavigationInfo.Previous.length-1){
                                    result += "<div class='arrowbread'></div>";
                                 }
                      result += "</div>";
                  }
               }
    result += "</div>";
    return result;
}



function getPreviousNavigation() {
    var result = [];
    for (var i = 0; i < window.requestInformation.NavigationInfo.Previous.length; i++)
        result[i] = window.requestInformation.NavigationInfo.Previous[i].RowId;
    return result;
}

