import cgi 
import cgitb
import os
import json
cgitb.enable()
from productionplan import resolve

form = cgi.FieldStorage()
print("Content-type: text/html; charset=utf-8\n")

html = """<!DOCTYPE html>
<head>
    <title>Mon programme</title>
</head>
<body>
    <form enctype="multipart/form-data" action="/index.py" method="post">
        <input type="file" name="file_name" s/>
        <input type="submit" name="send" value="Envoyer information au serveur">
    </form> 
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
    json.dump(solution,fp)

    
fp = open('response.json', 'w')
file_ = retrieve_json()
if file_ == -1 :
    print("No Datas . . .")
else :
    solution = resolve(file_)
    generate_json(fp, solution)

fp.close()