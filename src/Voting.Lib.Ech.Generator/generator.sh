#!/bin/bash

set -e  # Exit on error

script_dir="$(dirname "$(realpath "$0")")"
models_dir="$script_dir/Models"
project_dir_prefix="$script_dir/../Voting.Lib.Ech."

# Ensure xscgen is available
dotnet tool restore

# clean existing models
rm -rf "${models_dir}"

# List of namespace aliases
# All unique xml namespace => c# namespace mappings can be listed here
# if only the patch version differs, they need to be listed below
declare -a namespaces=(
    "http://www.ech.ch/xmlns/eCH-0006/2=Ech0006_2_0" \
    "http://www.ech.ch/xmlns/eCH-0006/3=Ech0006_3_0" \
    "http://www.ech.ch/xmlns/eCH-0007/5=Ech0007_5_0" \
    "http://www.ech.ch/xmlns/eCH-0007/6=Ech0007_6_0" \
    "http://www.ech.ch/xmlns/eCH-0008/3=Ech0008_3_0" \
    "http://www.ech.ch/xmlns/eCH-0010/5=Ech0010_5_1" \
    "http://www.ech.ch/xmlns/eCH-0010/6=Ech0010_6_0" \
    "http://www.ech.ch/xmlns/eCH-0010/7=Ech0010_7_0" \
    "http://www.ech.ch/xmlns/eCH-0010/8=Ech0010_8_0" \
    "http://www.ech.ch/xmlns/eCH-0011/8=Ech0011_8_1" \
    "http://www.ech.ch/xmlns/eCH-0011/9=Ech0011_9_0" \
    "http://www.ech.ch/xmlns/eCH-0021/7=Ech0021_7_0" \
    "http://www.ech.ch/xmlns/eCH-0021/8=Ech0021_8_0" \
    "http://www.ech.ch/xmlns/eCH-0044/4=Ech0044_4_1" \
    "http://www.ech.ch/xmlns/eCH-0045/4=Ech0045_4_0" \
    "http://www.ech.ch/xmlns/eCH-0045/6=Ech0045_6_0" \
    "http://www.ech.ch/xmlns/eCH-0046/4=Ech0046_4_0" \
    "http://www.ech.ch/xmlns/eCH-0058/5=Ech0058_5_0" \
    "http://www.ech.ch/xmlns/eCH-0072/1=Ech0072_1_0" \
    "http://www.ech.ch/xmlns/eCH-0097/4=Ech0097_4_0" \
    "http://www.ech.ch/xmlns/eCH-0098/4=Ech0098_4_0" \
    "http://www.ech.ch/xmlns/eCH-0110/4=Ech0110_4_0" \
    "http://www.ech.ch/xmlns/eCH-0135/1=Ech0135_1_0" \
    "http://www.ech.ch/xmlns/eCH-0135/2=Ech0135_2_0" \
    "http://www.ech.ch/xmlns/eCH-0155/4=Ech0155_4_0" \
    "http://www.ech.ch/xmlns/eCH-0157/4=Ech0157_4_0" \
    "http://www.ech.ch/xmlns/eCH-0157/5=Ech0157_5_1" \
    "http://www.ech.ch/xmlns/eCH-0159/4=Ech0159_4_0" \
    "http://www.ech.ch/xmlns/eCH-0159/5=Ech0159_5_1" \
    "http://www.ech.ch/xmlns/eCH-0222/1=Ech0222_1_0" \
    "http://www.ech.ch/xmlns/eCH-0222/3=Ech0222_3_0" \
    "http://www.ech.ch/xmlns/eCH-0228/1=Ech0228_1_0" \
    "http://www.ech.ch/xmlns/eCH-0252/1=Ech0252_1_0" \
    "http://www.ech.ch/xmlns/eCH-0252/2=Ech0252_2_0"
)
  
# expand namespaces with -n
all_namespaces="${namespaces[@]/#/-n }"

generate() {
    local schema_name="$1"
    local schema_file_name="$2"
    shift 2

    echo "Generating $schema_name"
    dotnet xscgen "${script_dir}/Schemas/${schema_file_name}" \
        --netCore \
        --separateFiles \
        --output="$models_dir" \
        --nullable \
        --order \
        --serializeEmptyCollections \
        --collectionSettersMode=Public \
        --generatedCodeAttribute- \
        --collectionType="System.Collections.Generic.List\`1" \
        --commandArgs- \
        $all_namespaces $@
}

# Generate classes for each schema
# if an eCH with different patch versions is used,
# it needs to be passed here.
generate "AbxVoting_1_0" "ABX-Voting-1-0.xsd" -n http://www.abraxas.ch/xmlns/ABX-Voting/1=AbxVoting_1_0
generate "AbxVoting_1_5" "ABX-Voting-1-5.xsd" -n http://www.abraxas.ch/xmlns/ABX-Voting/1=AbxVoting_1_5
generate "Ech0045_4_0" "eCH-0045-4-0.xsd"  -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0045_6_0" "eCH-0045-6-0.xsd"  -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0072_1_0" "eCH-0072-1-0.xsd"
generate "Ech0110_4_0" "eCH-0110-4-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0157_4_0" "eCH-0157-4-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0157_5_1" "eCH-0157-5-1.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0159_4_0" "eCH-0159-4-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0159_5_1" "eCH-0159-5-1.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0228_1_0" "eCH-0228-1-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0222_3_0" "eCH-0222-3-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_1
generate "Ech0252_1_0" "eCH-0252-1-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_0
generate "Ech0252_2_0" "eCH-0252-2-0.xsd" -n http://www.ech.ch/xmlns/eCH-0155/5=Ech0155_5_2

echo "Copying files into C# projects"
for dir in "$models_dir"/*/; do
    directory_name=$(basename "$dir" | tr -d '[:space:]')
    target_dir="${project_dir_prefix}${directory_name}/${directory_name}"
    
    cp -r "$dir"* "$target_dir"
done

echo "Done"
