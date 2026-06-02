using System.Collections.Generic;


// ── MinigameType assignments ──────────────────────────────────────────────────────────
//   FlappyBird: Redshift Accelerator, Psyche Cannonball, Solar Electric Propulsion
//   MatchCards: Documentation, Reality Rewiring Bay, Powerpoint Presentation
//   WhackAMole: Spectrometer of Truth, Solar-Powered Shock Trap, Cosmic Chatroom
//   SpaceInvaders: Jovian Fastball, Solar Panel Nunchucks, Deep Space Blaster
//   RhythmQTE: Drum Kit, Deep Space Hotline
//   RollingDice: Lucky Peanuts, Tap Out, Bribery, Big Red Button

/// <summary>
/// Stat scale: each filled star = 1 pt, each empty icon = 0 pt (max 5).
/// Scientific Accuracy is shown only when non-zero
/// </summary>

public static class CardDatabase
{
    public static List<AnswerCard> GetAllAnswerCards()
        => new List<AnswerCard>(_answerCards);

    public static List<AnswerCard> GetCardsByMinigame(MinigameType type)
        => _answerCards.FindAll(c => c.associatedMinigame == type);

    public static List<AnswerCard> GetNonSpecialCards()
        => _answerCards.FindAll(c => !c.IsSpecial());

    public static List<PromptCard> GetAllPromptCards()
        => new List<PromptCard>(_promptCards);

    public static AnswerCard GetAnswerCardById(int id)
        => _answerCards.Find(c => c.id == id);

    public static PromptCard GetPromptCardById(int id)
        => _promptCards.Find(c => c.id == id);
    
    // ── Answer Cards ──────────────────────────────────────────────────────────
    // Constructor: (id, title, description, effectiveness, chaos, scientificAccuracy, baseScore, minigame)
    // baseScore = effectiveness + chaos + scientificAccuracy

    private static readonly List<AnswerCard> _answerCards = new List<AnswerCard>
    {
        new AnswerCard(1,
            "Invisibility Cloak",
            "Wrap yourself in a carefully positioned array of Psyche's solar electric thrusters, ionizing xenon gas around you to create a shimmering plasma field. Does it actually make you invisible? No. But it does make you look like a ghostly space wizard.",
            0, 2, 1, 3),

        new AnswerCard(2,
            "Jovian Fastball",
            "With reckless ingenuity and no regard for orbital mechanics, Psyche hurls one of Jupiter's many moons straight at the threat.",
            3, 4, 1, 8,
            MinigameType.SpaceInvaders),

        new AnswerCard(3,
            "Redshift Accelerator",
            "By cleverly manipulating the spectrometer, Psyche amplifies an object's redshift, convincing the universe it's already in rapid retreat. The result? Psyche actually takes off at high speed. Problem solved, with physics left slightly perplexed.",
            3, 5, 1, 9,
            MinigameType.FlappyBird),

        new AnswerCard(4,
            "Systems Engineering",
            "When in doubt, throw a flowchart at it. With enough requirements, trade studies, and design reviews, even an alien invasion can be designed out of existence.",
            1, 0, 0, 1),

        new AnswerCard(5,
            "Documentation",
            "Well, the answer's in the documentation somewhere, I'm sure. We did make sure to document everything...right?",
            0, 1, 0, 1,
            MinigameType.MatchCards),

        new AnswerCard(6,
            "Lucky Peanuts",
            "Feeling a bit shaky? Time to channel some JPL luck with their famous peanuts. If they worked for Viking and Curiosity, I'm sure they'll work here too!",
            3, 1, 0, 4,
            MinigameType.RollingDice),

        new AnswerCard(7,
            "Tap Out",
            "It works in Jiu Jitsu, why not in deep space? If the problem's too tough, just gracefully tap out and hope Mission Control has your back.",
            0, 2, 0, 2,
            MinigameType.RollingDice),

        new AnswerCard(8,
            "PowerPoint Presentation",
            "A well-placed PowerPoint can convince physics or even an alien civilization that everything is going exactly as planned. Bonus points for adding unnecessary animations.",
            2, 1, 0, 3,
            MinigameType.MatchCards),

        new AnswerCard(9,
            "Descope",
            "If we descope enough, maybe they won't see us as a threat—just a friendly spacecraft minding its own business! Plus, if we never intended to do science, is it really a failure?",
            4, 0, 0, 4),

        new AnswerCard(10,
            "Premature Powers",
            "The spacecraft confidently taps into the mighty forces of asteroid Psyche—despite being millions of miles away. Maybe it's psychic? Maybe it's just wishful thinking? Either way, with a Massachusetts-sized metal asteroid as its imaginary power source, Psyche is feeling unstoppable.",
            1, 2, 0, 3),

        new AnswerCard(11,
            "Starlink Backup Crew",
            "Psyche remembers the many Starlink launches before and after its own and decides to call in a favor. A swarm of friendly internet satellites zips in to assist—whether it's relaying messages, providing moral support, or just making space feel a little less lonely.",
            3, 5, 0, 8),

        new AnswerCard(12,
            "Bribery",
            "Offer the intruder some redundant spacecraft parts and pray they don't realize they're getting scammed.",
            2, 5, 0, 7,
            MinigameType.RollingDice),

        new AnswerCard(13,
            "Reality Rewiring Ray",
            "Adjusts the fundamental laws of physics... for better or worse. Who needs gravity all the time, anyway?",
            3, 5, 1, 9,
            MinigameType.MatchCards),

        new AnswerCard(14,
            "Hyper-Evolved Goldfish",
            "A tiny fish in a floating water sphere who somehow understands orbital mechanics better than you do.",
            3, 2, 0, 5),

        new AnswerCard(15,
            "No-Fly Zone",
            "Just tell any threats they're not allowed here. Maybe they'll respect space bureaucracy.",
            0, 2, 0, 2),

        new AnswerCard(16,
            "Big Red Button",
            "There's always a Big Red Button. No one knows what it does. No one dares press it. But desperate times call for chaotic solutions. Initiating... something.",
            2, 5, 0, 7,
            MinigameType.RollingDice),

        new AnswerCard(17,
            "Time-Traveler",
            "They've seen the future, lived the past, and charge way too much for advice. Will their cryptic warnings save the mission—or just leave you more confused?",
            3, 3, 0, 6),

        new AnswerCard(18,
            "The Egg Cracks",
            "Turns out Psyche wasn't just a metal asteroid—it was an egg, and whatever just hatched is very interested in helping...or at least really good at smashing things. Deploy your newfound alien ally to fix the problem!",
            4, 5, 0, 9),

        new AnswerCard(19,
            "The Alcubierre Maneuver",
            "Who needs fuel when you can just bend spacetime? Engage the warp bubble, break physics, and hope the universe doesn't notice.",
            3, 4, 0, 7),

        new AnswerCard(20,
            "Brute Force Calculation",
            "If you throw enough computing power at a problem, it has to solve itself...right? Just pray your processor doesn't melt.",
            2, 1, 0, 3),

        new AnswerCard(21,
            "The Boltzmann Brain",
            "What if you're just a random, self-aware fluctuation in deep space? If so, none of this is real... but hey, no real problem, either!",
            2, 1, 0, 3),

        new AnswerCard(22,
            "Borrowed Time",
            "Who needs redundancies when you need more juice NOW? Just don't think too hard about what systems you just sacrificed…",
            4, 5, 0, 9),

        new AnswerCard(23,
            "Reverse the Polarity!",
            "Nobody actually knows what this means, but it always sounds like the right move. Flip every setting to its exact opposite and just... see what happens.",
            1, 5, 0, 6),

        new AnswerCard(24,
            "Frisbee of Doom",
            "Originally designed for deep-space communication, this high-gain X-band antenna also happens to be perfectly shaped for high-velocity disc throws. Whether you need to send a signal or send something flying, it's got range.",
            4, 5, 0, 9),

        new AnswerCard(25,
            "Solar Panel Nunchucks",
            "Fold 'em, snap 'em, and now Psyche's massive solar arrays are deadly in the right hands. Great for generating power and taking down space bandits.",
            3, 4, 0, 7,
            MinigameType.SpaceInvaders),

        new AnswerCard(26,
            "Deep Space Blaster",
            "NASA's Deep Space Optical Comm laser was built for high-bandwidth data transmission, but it also happens to be a great way to blind an enemy spacecraft or mess with an alien cat from 200 million miles away.",
            2, 3, 0, 5,
            MinigameType.SpaceInvaders),

        new AnswerCard(27,
            "Drum Kit",
            "Bang on the reaction wheels, tap the solar array panels, and turn the high-gain antenna into a cymbal. If comms are down, at least you can signal with sick beats.",
            0, 2, 0, 2,
            MinigameType.RhythmQTE),

        new AnswerCard(28,
            "Solar Panel Origami",
            "Fold and reshape the solar arrays into giant paper cranes. Not useful, but a nice way to pass the time in deep space.",
            0, 1, 0, 1),

        new AnswerCard(29,
            "Quantum Coin Flip",
            "A 50/50 chance of fixing everything... or making it infinitely worse.",
            3, 3, 0, 6),

        new AnswerCard(30,
            "Pocket-Sized Wormhole",
            "Finally, an escape plan! Too bad it only works for very small objects.",
            3, 1, 1, 5),

        new AnswerCard(31,
            "Suspiciously Helpful AI",
            "Claims it doesn't want to take over the ship. Probably lying.",
            4, 4, 0, 8),

        new AnswerCard(32,
            "Summon the Ancient Space Bureaucracy",
            "They will take centuries to process your distress call, but hey, it's something.",
            0, 2, 0, 2),

        new AnswerCard(33,
            "A Shameless Plug",
            "Desperate times call for desperate measures. Psyche quickly broadcasts a message: 'Follow @MissionToPsyche on Instagram for real-time updates and exclusive content!'",
            0, 3, 0, 3),

        new AnswerCard(34,
            "Solar Electric Propulsion",
            "Psyche's Hall-effect thrusters fire out charged xenon ions, creating a beautiful blue glow... and about as much thrust as holding an AA battery. Slow? Yes. Cool? Absolutely. Also, if an intruder shows up, we can lightly push them away over the course of several weeks.",
            2, 1, 0, 3,
            MinigameType.FlappyBird),

        new AnswerCard(35,
            "Multispectral Selfie Stick",
            "Why settle for a regular photo when you can capture your asteroid adventure in every wavelength? Perfect for revealing hidden secrets—or just finding out if Psyche looks better in infrared.",
            3, 1, 0, 4),

        new AnswerCard(36,
            "Magnetometer",
            "Whether it's Psyche's ancient magnetic field or a rogue piece of space junk, this trusty sensor will sniff it out.",
            4, 1, 0, 5),

        new AnswerCard(37,
            "Gravitational Wi-Fi",
            "By tracking tiny shifts in radio waves, we can map Psyche's gravity, mass, and rotation—basically using deep-space Wi-Fi to weigh an asteroid. Also handy for detecting invisible forces... like sneaky aliens.",
            5, 2, 0, 7),

        new AnswerCard(38,
            "Spectrometer of Truth",
            "Point, scan, and instantly reveal what that alien—or suspiciously aggressive space debris—is made of. Knowledge is power, and sometimes, power means knowing if you should run.",
            4, 2, 0, 6,
            MinigameType.WhackAMole),

        new AnswerCard(39,
            "Psyche Cannonball",
            "At over 6,000 pounds, Psyche may not be the fastest, but it is the heaviest thing around. If all else fails, just fire the thrusters and barrel straight through the problem, DART-style.",
            5, 5, 0, 10,
            MinigameType.FlappyBird),

        new AnswerCard(40,
            "Deep Space Hotline",
            "Psyche stays in touch using four antennas and NASA's Deep Space Network. Perfect for sending science data, receiving commands, and desperately calling home when things go very wrong.",
            3, 1, 0, 4,
            MinigameType.RhythmQTE),

        new AnswerCard(41,
            "Too Big to Fail",
            "With its solar panels deployed, Psyche stretches 81 feet—about the size of a tennis court. Great for collecting sunlight, terrible for fitting into tight spaces. If something needs swatting, though... we've got reach.",
            3, 4, 0, 7),

        new AnswerCard(42,
            "Solar-Powered Shock Trap",
            "Psyche's solar arrays generate up to 21 kilowatts of power—plenty for running instruments, charging systems... and, if necessary, giving an intruder the world's first deep-space zap.",
            4, 4, 0, 8,
            MinigameType.WhackAMole),

        new AnswerCard(43,
            "Laser-Graffiti",
            "Psyche's Deep Space Optical Comm wasn't designed to be a weapon, but with the right focus, it could etch a very strongly worded message onto an approaching intruder's hull.",
            3, 3, 0, 6),

        new AnswerCard(44,
            "Deep Space Metal Detector",
            "Psyche's gamma-ray and neutron spectrometer can analyze asteroid material by detecting emitted neutrons and gamma rays. No smuggling alien alloys on this mission!",
            4, 1, 0, 5),

        new AnswerCard(45,
            "Magnetic Shield",
            "This magnetometer isn't just looking for ancient fields—it's on high alert, using magnetic forces to keep space debris and intruders at bay.",
            3, 3, 0, 6),

        new AnswerCard(46,
            "Cosmic Chatroom",
            "Using Psyche's antennas and NASA's Deep Space Network, this system sends messages not just to Earth, but to the farthest reaches of space. Whether it's science data or chatting with aliens, Psyche's got you covered—unless it's spam.",
            4, 2, 0, 6,
            MinigameType.WhackAMole),

        new AnswerCard(47,
            "Planetary Swing",
            "Psyche harnesses the gravitational pull of nearby planets to take out space intruders or get around unexpected obstacles. Need to speed up or throw off a space pirate? Just slingshot around a planet for some extra oomph.",
            5, 5, 0, 10),

        new AnswerCard(48,
            "Slow Down...Everything Else",
            "Use pure adrenaline to move so fast that time itself starts to bend! Now that you have time, figure out how to actually fix the issue.",
            4, 5, 0, 9),

        new AnswerCard(49,
            "Titanium Repair Kit",
            "Using the tough, lightweight Ti6Al4V components made by Maxar, Psyche can quickly repair or reinforce parts of itself during the journey. These strut-truss structures bring strength, lightness, and flexibility—ideal for repairing a damaged part or reinforcing the spacecraft mid-flight.",
            5, 1, 0, 6)
    };

    // ── Prompt Cards  ──────────────────────────────────────────────────────────

    private static readonly List<PromptCard> _promptCards = new List<PromptCard>
    {
        new PromptCard(1,
            "A micrometeoroid just punctured a piece of thermal insulation! Temperatures are dropping fast. What's the spacecraft's emergency fix?",
            new Preferences(3, 2, 3), 3, 2, 3),

        new PromptCard(2,
            "Telemetry is showing an \"UNKNOWN OBJECT ATTACHED.\" Do something.",
            new Preferences(3, 3, 2), 3, 3, 2),

        new PromptCard(3,
            "A rogue space probe just planted a tiny flag on Psyche and declared it its own. How does the spacecraft negotiate?",
            new Preferences(3, 4, 1), 3, 4, 1),

        new PromptCard(4,
            "A software update mid-mission just turned Psyche into a text-based adventure game. The only way to win is _____.",
            new Preferences(2, 5, 1), 2, 5, 1),

        new PromptCard(5,
            "The high-gain antenna is now only transmitting memes. Mission Control will have to rely on _____ for communication instead.",
            new Preferences(2, 4, 2), 2, 4, 2),

        new PromptCard(6,
            "The amount of dark energy just doubled, and now the universe is expanding twice as fast. Psyche is being yanked away at an alarming rate! The spacecraft will need _____ to fight back.",
            new Preferences(3, 5, 2), 3, 5, 2),

        new PromptCard(7,
            "A swarm of unidentified objects is approaching at high speed. The only way to defend the spacecraft is with _____.",
            new Preferences(4, 3, 2), 4, 3, 2),

        new PromptCard(8,
            "Mission Control has detected something following Psyche through deep space. Either it's a fan or a problem. Time to use _____ to shake it off.",
            new Preferences(3, 4, 2), 3, 4, 2),

        new PromptCard(9,
            "The fabric of reality itself is starting to glitch around the spacecraft. Psyche's last hope is _____.",
            new Preferences(2, 5, 2), 2, 5, 2),

        new PromptCard(10,
            "The spacecraft just declared itself sentient and refuses to follow commands. Our only option is _____.",
            new Preferences(3, 5, 1), 3, 5, 1),

        new PromptCard(11,
            "Psyche has been challenged to a high-stakes game of space poker. If it loses, the spacecraft is forfeit. The best strategy is _____.",
            new Preferences(3, 4, 2), 3, 4, 2),

        new PromptCard(12,
            "A time traveler just arrived on board and is extremely confused. The best way to handle this is _____.",
            new Preferences(2, 4, 2), 2, 4, 2),

        new PromptCard(13,
            "Due to an engineering oversight, Psyche's solar panels have become too powerful and are now attracting confused moths from across the galaxy. The only way to stop them is _____.",
            new Preferences(2, 5, 2), 2, 5, 2),

        new PromptCard(14,
            "Psyche's star trackers just mistook a bright asteroid for the Sun and are now completely lost. The best way to recalibrate is _____.",
            new Preferences(4, 2, 3), 4, 2, 3),

        new PromptCard(15,
            "A solar flare just caused a bit flip in the onboard computer, and now it insists it was designed to land on Psyche instead of orbit. To convince it otherwise, we must deploy _____.",
            new Preferences(4, 3, 2), 4, 3, 2),

        new PromptCard(16,
            "The cold gas thruster is stuck open, slowly bleeding out Psyche's propellant. What's the fix?",
            new Preferences(5, 2, 3), 5, 2, 3),

        new PromptCard(17,
            "The neutron spectrometer just picked up unexpected hydrogen signatures in deep space. Uh oh, the aliens have water guns!",
            new Preferences(3, 5, 2), 3, 5, 2),

        new PromptCard(18,
            "Psyche's 24.7-meter-long solar panels have somehow become wedged between two asteroids. How does the spacecraft break free?",
            new Preferences(4, 3, 2), 4, 3, 2),

        new PromptCard(19,
            "A mysterious bright source is throwing off Psyche's sun sensor. How does the spacecraft recalibrate and get back on course?",
            new Preferences(4, 2, 3), 4, 2, 3),

        new PromptCard(20,
            "The Deep Space Optical Communications (DSOC) accidentally transmitted a cat video to an alien spacecraft. Now they're heading toward Psyche, convinced the video is a hostile message.",
            new Preferences(2, 5, 2), 2, 5, 2),

        new PromptCard(21,
            "Psyche's sensors just detected spice in deep space. Now a fleet of sandworms is on its way. What now?",
            new Preferences(3, 5, 1), 3, 5, 1),

        new PromptCard(22,
            "A mysterious wormhole just opened in Psyche's path. It's either a shortcut or a one-way ticket to a fourth-dimensional bookshelf. To navigate this, Psyche will need _____.",
            new Preferences(3, 4, 2), 3, 4, 2),

        new PromptCard(23,
            "Mission control is asking if you can 'science the [REDACTED] out of this' after Psyche's main power system went down. What's the plan?",
            new Preferences(5, 2, 4), 5, 2, 4),

        new PromptCard(24,
            "Psyche just got a message from a stranded astronaut on Mars. The only way to help is with _____.",
            new Preferences(4, 3, 3), 4, 3, 3),

        new PromptCard(25,
            "Uh oh. Psyche's thrusters just fired...on their own...and now it's spinning uncontrollably. Quick! Use _____ to stabilize it!",
            new Preferences(5, 3, 3), 5, 3, 3),

        new PromptCard(26,
            "The solar panels are covered in space dust! Power is dropping fast. What's the best way to clean them off?",
            new Preferences(4, 2, 3), 4, 2, 3),

        new PromptCard(27,
            "Psyche just accidentally offended an advanced Type III alien species. The only way to make peace is with _____.",
            new Preferences(3, 4, 2), 3, 4, 2),

        new PromptCard(28,
            "Psyche flew too close to a rogue black hole! The ship is being pulled in. What's the last-ditch escape plan?",
            new Preferences(4, 4, 2), 4, 4, 2),

        new PromptCard(29,
            "Psyche is suffering from existential dread. It has begun questioning its purpose in the universe. Maybe _____ will snap Psyche out of it?",
            new Preferences(2, 4, 2), 2, 4, 2),
    };
}