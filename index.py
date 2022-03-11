import cgi 
import cgitb
import os
import json
cgitb.enable()
from powerplant_planning import resolve

form = cgi.FieldStorage()
print("Content-type: text/html; charset=utf-8\n")

html = """<!DOCTYPE html>
<head>
    <title>Mon programme</title>
    <style>
        body{
            display:flex;
            flex-direction : column;
        }
        #pic1{
            float: right;
            padding:10px;
            flex:1;
            }
        header{
            flex : 1;
            display : flex;
            flex-direction: row;
            margin:0px;
            margin-bottom: 10%;
            border : solid black;
            background-color : #FFB81C;
        }
        h1
        {
            margin: auto;
            padding: auto;
            padding-left: 30%;
            flex : 5;
            height: 100%;
        }
        
        #form_div{
            flex : 2;
            margin-left: 20%;
            margin-bottom: 10%;
        }
    </style>

</head>
<body>
    <header>
        <img float=left src="image/akkodis.png" alt="Akkodis Logo" class="pic1" id="pic1"/>
        <h1>Rayan Benhamana</h1>
    </header>
    
    <section id="form_div">
        <form enctype="multipart/form-data" action="/index.py" method="post">
            <input type="file" name="file_name" s/>
            <input type="submit" name="send" value="Envoyer information au serveur">
        </form> 
    </section>
</body>
</html>
"""

print(html)
   

def retrieve_json() :
    file= form.getvalue("file_name")
    if file :
        file = file.decode()
        file = json.loads(file)
        return file
    else :
        return -1

def generate_json(fp, solution) :
    solution_json = []
    for name in solution :
        tmp_json = {}
        tmp_json["name"] = name
        tmp_json["p"] = solution[name]["prod"] 
        solution_json.append(tmp_json)
    json.dump(solution_json,fp)

    
fp = open('response.json', 'w')
file_ = retrieve_json()
if file_ == -1 :
    print("No Datas . . .")
else :
    solution = resolve(file_)
    generate_json(fp, solution)

fp.close()