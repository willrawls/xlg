@ECHO OFF
tlbexp "%1"
regasm /codebase "%1"
echo done
