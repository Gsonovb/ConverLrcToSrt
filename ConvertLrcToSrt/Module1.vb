Imports System.IO
Imports System.Collections.Generic
Imports System.Text

Module MainModule

    Sub Main()


        Dim dir = Directory.GetCurrentDirectory()

        Dim files = Directory.EnumerateFiles(dir, "*.lrc")



        For Each file In files

            Dim filename = Path.ChangeExtension(file, ".srt")

            Convert(file, filename)

        Next




    End Sub

    Dim formats As String() = {"%m\:ss\.ff", "%h\:%m\:ss\.ff"}
    Dim dts As TimeSpan = TimeSpan.FromMilliseconds(50)
    'Dim ssts As TimeSpan = TimeSpan.FromMilliseconds(10)

    'Dim maxts As TimeSpan = TimeSpan.FromMilliseconds(250)

    Dim ets As TimeSpan = TimeSpan.FromSeconds(10)



    Sub Convert(inputfilename As String, outputfilename As String)

        Console.WriteLine("正在转换文件{0} 到 {1} ...", inputfilename, outputfilename)

        Dim sb As New StringBuilder()

        Dim lines = File.ReadAllLines(inputfilename, Encoding.Default).ToList()




        Dim srts = lines.Select(Function(s)
                                    Dim index = lines.IndexOf(s) + 1
                                    Dim parts = s.Split("[]".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

                                    Dim ts = TimeSpan.ParseExact(parts(0), formats, System.Globalization.CultureInfo.InvariantCulture)

                                    Dim text = parts(1)

                                    Return New With {.Index = index, .Time = ts, .Text = text}

                                End Function)



        '1
        '00:00:00,000 --> 00:00:03,820
        '使用 HTML5 设计辅助功能

        Dim count = srts.Count

        For i = 0 To count - 1

            Dim cItem = srts(i)

            Dim tts = If(i + 1 >= count,
                         cItem.Time.Add(ets),
                         srts(i + 1).Time.Subtract(dts))


            sb.AppendLine(cItem.Index)

            Dim stime = String.Format("{0:hh\:mm\:ss\,fff} --> {1:hh\:mm\:ss\,fff}", cItem.Time, tts)

            sb.AppendLine(stime)

            sb.AppendLine(cItem.Text)

            sb.AppendLine()





        Next


        File.WriteAllText(outputfilename, sb.ToString)



    End Sub

End Module
