search_dir=example_payloads
for entry in "$search_dir"/*
do
  echo "Testfile: $entry"
  curl -X POST -H 'Content-Type: application/json' -d @"$entry" http://127.0.0.1:8888/productionplan
  echo -e ""
  echo -e "****************************************"
done
echo -e ""