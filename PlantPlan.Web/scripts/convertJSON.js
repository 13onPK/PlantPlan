function run(what, p_input, p_output) {
    var delimiter = ",";
    var noMultiLines = true; //document.getElementById('chkNoBreaks').checked;
    var whichTable = "1"; //document.getElementById('selTabNum').value;
    var forceHeader = false; //document.getElementById('chkForceHeader').checked;
    whichTable = whichTable || "0";
    var bQuotes = false;
    var removeTags = true; //(document.getElementById('chkRemoveTags')).checked;
    var html=document.getElementById('divHtml');
    // html.innerHTML=document.getElementById('txt1').value.replace(/<script/gmi,"<xxxxx");
    // html.innerHTML=document.getElementById('txt1').value.replace(/<style/gmi,"<yyyyy");
    html.innerHTML = p_input.replace(/<script/gmi, "<xxxxx");
    html.innerHTML = p_input.replace(/<style/gmi, "<yyyyy");


    var s="";
    var cells;
    var value;
    var tbl = html.getElementsByTagName('table');
    var cnt=tbl.length;
    var re = new RegExp("<\/?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)\/?>",'igm');
    var headerFound=false;
    for(var j=0;j<tbl.length;j++) {
        if( (""+(j+1)) != whichTable && whichTable!="0") continue;
        rows=tbl[j].getElementsByTagName('tr');
        for(var k=0;k<rows.length;k++) {
            if (k==0) {
               cells=rows[k].getElementsByTagName('th'); 
               if(cells && cells.length>0)headerFound=true;
            }
            if ('querySelectorAll' in document) {
               cells=rows[k].querySelectorAll('td,th');
            } else {
               cells=rows[k].getElementsByTagName('td');
               if(!cells || cells.length==0) {
                  cells=rows[k].getElementsByTagName('th');
               }
            }

            for(var n=0;n<cells.length;n++) {
               value=cells[n].innerHTML;
               if(value==null)value="";else value+="";
               value = value.replace(/\r\n|\r|\n/gmi,' ');
               if(noMultiLines) value = value.replace(/\n|<br>|<br\/>|<br \/>/gmi,'\n');
               //else value = value.replace(/\r\n|\r|\n|<br>|<br\/>|<br \/>/gmi,'\n');
               if (removeTags) value=value.replace(re,''); 
               value=_.unescape(value);
               value=value.replace(/&nbsp;/gmi," ");
               value=value.trim();
               if(bQuotes) {
                  s += '"' + value.replace(/"/gmi, '""') + '"' + delimiter; 
               }
               else {
                  s+=value.toCsv(delimiter,'"') + delimiter; 
               } 
           }
           s=s.slice(0, delimiter.length*-1); // remove last delimiter
           s += "\n"; 
        } 
    }

   
    // // s contains csv, convert that to JSON.
    // //document.getElementById('txta').value = s; 
    p_output = s;
    // //if(cnt<1 && document.getElementById('txt1').value.trim() != "") {
    //  if(cnt<1 && p_input.trim() != "") {
    //    window.alert('No TABLE tag found in HTML. Please check your input.');
    // }
    // s = "<select id=\"selTabNum\"  onchange=\"run()\">";
    // s += "<option value=0>-All-</option>"
    // for(j=0;j<cnt;j++) {
    //    s+= "<option value=\"" + (j+1) + "\" ";
    //    if( (j+1)==whichTable) s+=" selected";
    //    s+= ">" + getOrdinal(j+1) + "</option>";
    // }
    // s+="</select>";
    // // document.getElementById("spanTabNum").innerHTML = s;
    // // document.getElementById('spanCount').innerHTML = "(Tables found: " + cnt + ")";

    var options = {"global":[]};
    options.forceWrap  = false; //(document.getElementById('chkCsvQuotes')).checked;
    options.nullIsNull = true; //document.getElementById("chkNullJson").checked;
    options.useFieldsData  = false; //(document.getElementById('chkFldData')).checked;
    options.fldPropName    = "fields"; //document.getElementById("txtFieldName").value || "fields";
    options.dataPropName   = "data"; //document.getElementById("txtDataName").value || "data";

    CSV.isFirstRowHeader = headerFound || forceHeader;
    // CSV.parse(document.getElementById('txta').value);
    CSV.parse(p_output);
    what = what || page.lastWhat;
        switch (what) {
          case 1: options.isKeyed    = false;
                //   document.getElementById('txta').value = csvToJSON(CSV,options);
                  p_output = csvToJSON(CSV,options);
                  return p_output;
                  break;
          case 2: options.isKeyed    = true;
                //   document.getElementById('txta').value = csvToJSON(CSV,options);
                  p_output = csvToJSON(CSV,options);
                  break;
        //   case 3: document.getElementById('txta').value = csvToJSONArray(CSV,options);
            case 3: p_output = csvToJSONArray(CSV,options);
                  break;
         //   case 4: document.getElementById('txta').value = csvToJSONColumnArray(CSV,options);
          case 4: p_output = csvToJSONColumnArray(CSV,options);
                  break;
        }
        //page.lastWhat = what;
}