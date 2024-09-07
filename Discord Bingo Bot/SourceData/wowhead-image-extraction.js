var imageUrls = "\"itemimageurl\"";
var imageNames = "\"itemimagename\"";
for (const tag of $x("//ins")) {
    const backgroundImage = window.getComputedStyle(tag, null).getPropertyValue('background-image');
    const parseBgImage = backgroundImage.replace('url("', "").replace('")', "").replace("/medium/","/large/");
    const url = parseBgImage;
    const name = parseBgImage.substring(parseBgImage.lastIndexOf("/") + 1);
    if (url.indexOf("/small/") >= 0) {
        continue;
    }
    imageUrls += `,"${url}"`;
    imageNames += `,"${name}"`;
}
console.log(imageUrls);
console.log(imageNames);