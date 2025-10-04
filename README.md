# Ascii Slides
In progress, incomplete.

AsciiSlides is (will be?) a desktop tool for giving presentations. It parses plaintext files and let's you present them. In addition to supporting html, markdown, youtube links, and images; it also supports 'ASCII Art' and figlet style text headers. 

## Why?
1. Because ASCII art slides are pretty fun. It feels great to have presentations that look like 2000's era game guide text files
2. Splitting content authoring from presentation styling.  PowerPoint is fine, but I am very tired of the editing workflow. Just like using Markdown instead of writing in a WYSWIG editor; but for presentations
3. Speaker Notes and clocks are still really important to me
4. I want to create slides faster. I'm a Professor, I talk while standing near a big screen a lot. More tools is good

> This software isn't done yet. Maybe it never well be, making " Not a powerpoint a PowerPoint" is surely not a high priority for me. On the other hand, it does want to exist.

### Slides Table-Stakes
- Display slide content at an appropriate native resolution. 
- Speaker notes and a timer.
- Easily predict and determine display outputs. Easily set, swap, etc.
- Be able to navigate non-sequentially without leaving the presentation view.
- Must be able to run and be written, fully featured, without an internet connection

I want these:
- Free and open source
- Work even when displays are set to duplicated
- Export to .pptx, .pdf, .html. 
- Run on 'guest' computers easily

Most alternatives use HTML and web technology. Which makes sense! But websites are bad at OS level things, and display output management is just an OS level thing.
I use a WebView (html+css!) to *render* the slides. But the software can also, say, call native functions! I know about all the connected displays, for example.

## Why Not?
Separating content and styling is not a novel concept. It's the foundational principle behind HTML+CSS!

The people at [Marp](https://marp.app/) get it. (Note to self: Marp seems pretty alright?). 

### PowerPoint isn't Free
PowerPoint got rid of it's free presentation viewer. Now you can no longer even *view* presentations without paying for office. Or installing LibreOffice and hoping it works well. (Hey, in LibeOffice's defense, it's pretty good at managing powerpoint files).

I've never used [Keynote](https://support.apple.com/keynote) but it isn't free either. [Figma slides](https://www.figma.com/slides/) is part of figma and so it isn't free either, right? Same for [Canva Presentations](https://www.canva.com/presentations/)

- [Sozi](https://sozi.baierouge.fr/) is free but I haven't tried it.
- [LibreOffice Impress](https://www.libreoffice.org/discover/impress/) remains the strongest FOSS competitor to PowerPoint, but it also - intentionally - works the same way.

### Websites are bad at presentations.
Is the website fullscreen or is the embedded video fullscreen? will hitting escape un-fullscreen a video or close the editor and bring me back to Google slides? It's clumsy and awkward. 
Speaker notes and clear monitor selections is also critical for presentation. PowerPoint users click one button and the software basically works every time. But when you watch a speaker fumbling... it's probably google slides. dragging a window around, it's mess.

*At least PowerPoint isn't a [website](https://powerpoint.cloud.microsoft/en-us/). Wait, oh no!*

The same issues persist with Canva presentations, prezi, and reveal js.

### I shouldn't need an internet connection or to run a local server to present with speaker notes.

- [Reveal.js](https://github.com/hakimel/reveal.js): "When used locally, this feature requires that reveal.js runs from a local web server."
- [Impress.js](https://github.com/impress/impress.js/): The speaker notes plugin breaks if one of the pages refreshes.
- [sli.dev](https://sli.dev/): "To enter the presenter mode, you can click the button in the navigation panel, or visit http://localhost:\<port\>/presenter."
- [mdx-deck](https://github.com/jxnblk/mdx-deck): also seems to run a local server for speaker notes.
- [webslides.tv](webslides.tv/): Doesn't seem to have speaker notes?
- [Obsidian Slides Plugin](https://help.obsidian.md/plugins/slides): I love you obsidian but I need speaker notes.

## Using the Terminal is cool! ... but I need more
I started this project aiming to target the terminal. But opening multiple terminals for speaker notes and connecting the windows... it got silly when "just write a desktop app" is *right there* as a solution.

- [TUI-Slides](https://github.com/Chleba/tui-slides) is cool!
- [Presenterm](https://github.com/mfontanini/presenterm) is also cool!
- [Patat](https://github.com/jaspervdj/patat) guess what? also cool.
