<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestProject.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
	<script type="text/javascript" charset="utf-8" src="/resources/js/jquery-1.12.4.min.js"></script>
    <script type="text/javascript">
        const url = new URL(window.location.href);
        const params = url.searchParams;

        var leId = params.get('leId');
        var srId = params.get('srId');
        var gId = params.get('gId')
        var tbSc = params.get('tbSc');

        var list;
        var total_ab = 0, total_hit = 0, total_rbi = 0, total_run = 0;
        var start = [];

        $(document).ready(function () {
            console.log('b');
            // 데이터
            //getData(leId, srId, gId, tbSc);

            $("#tbl1").find("#TEAM_NM").append(list.list[0].TEAM_NM);

            $.each(list.list, function (index, item) {
                
                total_ab += item.AB_CN;
                total_hit += item.HIT_CN;
                total_rbi += item.RBI_CN;
                total_run += item.RUN_CN;

                if (item.CH_INN_NO == 0) {
                    start[index] = '*';
                } else {
                    start[index] = '';
                }

                $("#tbl1").append(
                    "<tr>"
                    + "<td>" + start[index] + "</td>"
                    + "<td>" + item.POS_IF + "</td>"
                    + "<td>" + item.P_NM + "</td>"
                    + "<td>" + item.CH_INN_NO + "</td>"
                    + "<td>" + item.INN1_1_IF + "</td>"
                    + "<td>" + item.INN2_1_IF + "</td>"
                    + "<td>" + item.INN3_1_IF + "</td>"
                    + "<td>" + item.INN4_1_IF + "</td>"
                    + "<td>" + item.INN5_1_IF + "</td>"
                    + "<td>" + item.INN6_1_IF + "</td>"
                    + "<td>" + item.INN7_1_IF + "</td>"
                    + "<td>" + item.INN8_1_IF + "</td>"
                    + "<td>" + item.INN9_1_IF + "</td>"
                    + "<td>" + item.INN10_1_IF + "</td>"
                    + "<td>" + item.INN11_1_IF + "</td>"
                    + "<td>" + item.INN12_1_IF + "</td>"
                    + "<td>" + item.INN13_1_IF + "</td>"
                    + "<td>" + item.INN14_1_IF + "</td>"
                    + "<td>" + item.INN15_1_IF + "</td>"
                    + "<td>" + item.AB_CN + "</td>"
                    + "<td>" + item.HIT_CN + "</td>"
                    + "<td>" + item.RBI_CN + "</td>"
                    + "<td>" + item.RUN_CN + "</td>"
                    + "<td>" + item.SEASON_HRA_RT + "</td>"
                    + "<td>" + item.GAME5_HRA_RT + "</td>"
                    + "</tr>"
                    );
            })

            $("#tbl1").append(
                "<tr>"
                + "<td colspan='19'>Total</td>"
                + "<td>" + total_ab + "</td>"
                + "<td>" + total_hit + "</td>"
                + "<td>" + total_rbi + "</td>"
                + "<td>" + total_run + "</td>"
                + "<td>" + list.list3[0].HRA_RT_SEASON  + "</td>"
                + "<td>" + list.list2[0].HRA_RT_5 + "</td>"


            );

            console.log(list.list2[0].HRA_RT_5);
            

        });

        // 데이터 가져오기
        function getData(leId, srId, gId, tbSc) {
            var request = $.ajax({
                type: "post"
                , url: "/ws/Common.asmx/GetTest"
                , dataType: "json"
                , data: {
                    leId: leId
                    , srId: srId
                    , gId: gId
                    , tbSc: tbSc
                }
                , async: false
                , error: function (e) {
                    console.log(e);
                }
            });

            request.done(function (data) {
                console.log(data);
                list = data;
            });
        }


        
    </script>
</head>
<body>
    <table id="tbl1" border="1">
        <tr>
            <td rowspan="2" colspan="4" id="TEAM_NM"></td>
            <td rowspan="2">1</td>
            <td rowspan="2">2</td>
            <td rowspan="2">3</td>
            <td rowspan="2">4</td>
            <td rowspan="2">5</td>
            <td rowspan="2">6</td>
            <td rowspan="2">7</td>
            <td rowspan="2">8</td>
            <td rowspan="2">9</td>
            <td rowspan="2">10</td>
            <td rowspan="2">11</td>
            <td rowspan="2">12</td>
            <td rowspan="2">13</td>
            <td rowspan="2">14</td>
            <td rowspan="2">15</td>
            <td rowspan="2">타수</td>
            <td rowspan="2">안타</td>
            <td rowspan="2">타점</td>
            <td rowspan="2">득점</td>
            <td colspan="2">타율</td>
        </tr>
        <tr>
            <td>시즌</td>
            <td>5게임</td>
        </tr>
    </table>
</body>
</html>
