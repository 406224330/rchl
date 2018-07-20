echo "# rchl" >> README.md
git init
git add README.md
git add .
git commit -m "first commit"
git remote add origin https://github.com/bruceddlb/rchl.git
git pull origin master
git push -u origin master
