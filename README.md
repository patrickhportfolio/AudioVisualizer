# AudioVisualizer

I have created a few different audio visualizations to gain an understanding of processing in Unity. These modules could be easily modified to provide
dynamic visuals.

Currently there exists a traditional amplitude visual that separates the audio spectrum into bands and moves according to the current sample amplitude. The visual also glows depending on the ratio of the current band amplitude to the max that has occurred.

There si also the "wave" visual that extends the band version to a 3d nature. Here we can see the past few samples and how the amplitude is changing over time.

Finally, the "beat" visualizer. When a beat is sensed, the visual raises and extends outward like a drop of water on a pond. This component's can be customized in many ways in order to better pinpoint when a beat occurs since songs are mixed very differently.
