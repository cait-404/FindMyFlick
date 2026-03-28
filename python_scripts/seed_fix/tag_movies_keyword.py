"""
tag_movies_keyword.py

Keyword-matching script to associate plot tags with movies based on plot summaries.
Reads from the database, matches keywords, and inserts into movie_plot_tags.

Usage:
    python tag_movies_keyword.py --limit 10          # test batch of 10 movies
    python tag_movies_keyword.py --imdb tt0068421    # single specific movie
    python tag_movies_keyword.py --all               # all movies (run after testing)
"""

import argparse
import os
import psycopg2
from dotenv import load_dotenv

load_dotenv()

# ---------------------------------------------------------------------------
# Database connection
# ---------------------------------------------------------------------------
DB_CONFIG = {
    "host":     os.getenv("DB_HOST", "localhost"),
    "port":     int(os.getenv("DB_PORT", 5432)),
    "dbname":   os.getenv("DB_NAME", "findmyflick"),
    "user":     os.getenv("DB_USER", "postgres"),
    "password": os.getenv("DB_PASSWORD", "p@ssw0rd"),
}

# ---------------------------------------------------------------------------
# Keyword map: tag_text_norm -> list of keywords/phrases to look for
# The plot summary is lowercased before matching, so keep keywords lowercase.
# Add or expand entries as needed.
# ---------------------------------------------------------------------------
TAG_KEYWORDS = {
    "coming_of_age":            ["coming of age", "grows up", "growing up", "teenager", "adolescent", "young adult", "youth"],
    "redemption_arc":           ["redemption", "redeems", "second chance", "atone", "atonement", "makes amends"],
    "revenge":                  ["revenge", "vengeance", "avenge", "retribution", "payback"],
    "love_triangle":            ["love triangle", "torn between", "two suitors", "jealousy between lovers"],
    "forbidden_love":           ["forbidden love", "forbidden romance", "secret love", "star-crossed", "star crossed"],
    "fish_out_of_water":        ["fish out of water", "out of place", "doesn't fit in", "new world", "unfamiliar territory"],
    "rags_to_riches":           ["rags to riches", "rises from poverty", "from nothing", "self-made", "poor to rich"],
    "underdog_story":           ["underdog", "against all odds", "unlikely hero", "nobody believes", "prove everyone wrong"],
    "heros_journey":            ["hero's journey", "quest", "chosen", "ordinary world", "call to adventure"],
    "tragic_hero":              ["tragic hero", "fatal flaw", "downfall", "tragedy", "doomed"],
    "anti_hero":                ["anti-hero", "antihero", "morally ambiguous", "flawed protagonist", "reluctant savior"],
    "villain_origin_story":     ["villain origin", "becomes the villain", "how he became", "how she became", "turns evil"],
    "good_vs_evil":             ["good vs evil", "good versus evil", "forces of evil", "battle between good", "light and dark"],
    "moral_dilemma":            ["moral dilemma", "ethical choice", "impossible choice", "right and wrong", "conscience"],
    "identity_crisis":          ["identity crisis", "who am i", "sense of self", "who he is", "who she is", "lost himself", "lost herself"],
    "amnesia":                  ["amnesia", "memory loss", "can't remember", "forgotten past", "lost memories"],
    "secret_identity":          ["secret identity", "double life", "hidden identity", "alias", "disguise"],
    "hidden_past":              ["hidden past", "dark past", "secret past", "mysterious past", "past comes back"],
    "found_family":             ["found family", "unlikely family", "band of misfits", "family they chose", "chosen family"],
    "broken_family":            ["broken family", "dysfunctional family", "estranged", "broken home", "absent father", "absent mother"],
    "family_reunion":           ["family reunion", "reunited with family", "long-lost family", "reconnects with family"],
    "sibling_rivalry":          ["sibling rivalry", "brother against brother", "sister against sister", "siblings compete"],
    "mentor_student":           ["mentor", "apprentice", "student", "teacher", "trains him", "trains her", "takes him under", "takes her under"],
    "betrayal":                 ["betrayal", "betrayed", "betrays", "double cross", "stab in the back", "turns on"],
    "double_cross":             ["double cross", "double-cross", "double crosses", "set up", "played for a fool"],
    "heist":                    ["heist", "robbery", "bank job", "steal", "caper", "master thief", "crew assembles"],
    "conspiracy":               ["conspiracy", "cover-up", "cover up", "shadow organization", "secret plot", "puppet master"],
    "political_intrigue":       ["political intrigue", "political scheming", "power struggle", "political corruption", "government plot"],
    "espionage":                ["espionage", "spy", "spying", "intelligence agency", "covert operation", "classified"],
    "spy_thriller":             ["spy thriller", "secret agent", "double agent", "MI6", "CIA operative", "spy mission"],
    "time_travel":              ["time travel", "time machine", "travels back in time", "travels to the future", "time traveler"],
    "time_loop":                ["time loop", "relives", "stuck in a loop", "same day over", "groundhog"],
    "alternate_reality":        ["alternate reality", "alternate universe", "parallel world", "different timeline"],
    "parallel_universe":        ["parallel universe", "multiverse", "alternate dimension", "another dimension"],
    "multiverse":               ["multiverse", "multiple universes", "parallel selves"],
    "fate_vs_free_will":        ["fate", "destiny", "free will", "predetermined", "written in the stars"],
    "destiny_fulfilled":        ["destiny", "fulfills his destiny", "fulfills her destiny", "born for this", "meant to be"],
    "prophecy":                 ["prophecy", "prophesied", "foretold", "oracle", "chosen one"],
    "chosen_one":               ["chosen one", "the one", "prophesied hero", "destined to save"],
    "reluctant_hero":           ["reluctant hero", "doesn't want to", "forced to step up", "unlikely savior", "ordinary person"],
    "quest":                    ["quest", "journey to find", "journey to stop", "mission to", "sets out to"],
    "treasure_hunt":            ["treasure hunt", "hidden treasure", "buried treasure", "artifact", "ancient relic"],
    "survival":                 ["survival", "survive", "fight to survive", "struggle to survive", "life or death"],
    "disaster":                 ["disaster", "catastrophe", "earthquake", "tsunami", "hurricane", "volcano", "apocalypse"],
    "post_apocalyptic":         ["post-apocalyptic", "post apocalyptic", "after the end", "wasteland", "collapsed society"],
    "dystopia":                 ["dystopia", "dystopian", "oppressive regime", "totalitarian", "authoritarian society"],
    "artificial_intelligence":  ["artificial intelligence", "AI", "robot", "android", "machine learning", "sentient machine"],
    "robot_uprising":           ["robot uprising", "machines rebel", "robots take over", "AI rebellion", "robot revolution"],
    "human_vs_machine":         ["human vs machine", "man vs machine", "humans vs robots", "fight against AI"],
    "space_exploration":        ["space exploration", "space mission", "astronaut", "galaxy", "outer space", "spacecraft"],
    "alien_invasion":           ["alien invasion", "aliens invade", "extraterrestrial attack", "alien threat", "invasion from space"],
    "first_contact":            ["first contact", "first encounter with aliens", "meet aliens", "discover alien life"],
    "body_swap":                ["body swap", "switches bodies", "body switch", "trades places physically"],
    "transformation":           ["transforms", "transformation", "becomes something new", "changed forever", "metamorphosis"],
    "curse":                    ["curse", "cursed", "ancient curse", "under a spell", "hexed"],
    "supernatural_haunting":    ["haunting", "haunted", "ghost", "supernatural", "paranormal", "specter"],
    "possession":               ["possession", "possessed", "takes over his body", "takes over her body", "demonic"],
    "exorcism":                 ["exorcism", "exorcist", "cast out demon", "drive out evil", "demonic possession"],
    "monster_hunt":             ["monster hunt", "hunting a monster", "creature", "beast", "hunt down"],
    "vampire_story":            ["vampire", "vampires", "blood-sucking", "undead", "fangs"],
    "werewolf_story":           ["werewolf", "werewolves", "lycanthropy", "transforms into a wolf"],
    "ghost_story":              ["ghost", "spirit", "apparition", "haunted house", "poltergeist", "specter"],
    "psychological_horror":     ["psychological horror", "paranoia", "mind games", "psychological thriller", "gaslighting"],
    "slasher":                  ["slasher", "serial killer", "masked killer", "stalking victims", "killing spree"],
    "serial_killer":            ["serial killer", "serial murderer", "killing spree", "mass murderer", "predator"],
    "whodunit":                 ["whodunit", "who did it", "murder mystery", "suspect", "killer revealed"],
    "detective_mystery":        ["detective", "investigator", "mystery", "case", "clues", "investigation"],
    "noir":                     ["noir", "femme fatale", "hard-boiled", "private eye", "dark underbelly"],
    "crime_drama":              ["crime drama", "organized crime", "criminal", "gang", "mob", "cartel"],
    "courtroom_drama":          ["courtroom", "trial", "lawyer", "attorney", "verdict", "prosecution", "defense"],
    "legal_battle":             ["legal battle", "lawsuit", "court case", "legal fight", "justice system"],
    "prison_escape":            ["prison escape", "breaks out of prison", "escape from prison", "jailbreak", "escapes custody"],
    "corruption":               ["corruption", "corrupt", "bribery", "dirty cop", "crooked politician", "abuse of power"],
    "redemption_in_prison":     ["prison", "incarcerated", "behind bars", "sentence", "parole", "rehabilitation"],
    "war_story":                ["war", "battle", "combat", "soldier", "military", "front lines", "warfare"],
    "soldiers_journey":         ["soldier's journey", "soldier", "troops", "deployment", "tour of duty"],
    "veteran_trauma":           ["veteran", "PTSD", "war trauma", "post-war", "returning soldier"],
    "brotherhood_in_battle":    ["brotherhood", "band of brothers", "fellow soldiers", "unit", "comrades in arms"],
    "resistance_movement":      ["resistance", "rebels", "underground movement", "fight back", "uprising"],
    "revolution":               ["revolution", "revolutionary", "overthrow", "rebellion", "topple"],
    "civil_unrest":             ["civil unrest", "riots", "protest", "civil war", "uprising", "social upheaval"],
    "historical_drama":         ["historical", "based on true events", "period piece", "set in the", "19th century", "20th century"],
    "biographical":             ["biography", "biographical", "based on the life", "true story", "real person"],
    "rise_to_fame":             ["rise to fame", "becomes famous", "stardom", "overnight success", "breakthrough"],
    "fall_from_grace":          ["fall from grace", "loses everything", "downfall", "disgrace", "scandal"],
    "music_journey":            ["musician", "band", "music", "singer", "songwriter", "rock star", "concert"],
    "sports_underdog":          ["sports", "underdog team", "championship", "tournament", "athletic", "compete"],
    "championship_quest":       ["championship", "title", "compete for", "win the cup", "finals"],
    "rivalry":                  ["rivalry", "rivals", "fierce competition", "bitter enemy", "nemesis"],
    "training_montage":         ["training", "prepares", "learns to fight", "hones his skills", "hones her skills"],
    "competition":              ["competition", "contest", "tournament", "face off", "battle it out"],
    "workplace_drama":          ["workplace", "office", "coworkers", "boss", "job", "career", "colleagues"],
    "office_romance":           ["office romance", "coworker romance", "falls for his colleague", "falls for her colleague"],
    "corporate_greed":          ["corporate greed", "corporation", "CEO", "big business", "profit over people"],
    "startup_story":            ["startup", "entrepreneur", "founds a company", "builds a business", "tech company"],
    "midlife_crisis":           ["midlife crisis", "middle age", "questions his life", "questions her life", "existential"],
    "second_chances":           ["second chance", "fresh start", "new beginning", "starts over", "redemption"],
    "self_discovery":           ["self-discovery", "finds himself", "finds herself", "discovers who", "journey of self"],
    "road_trip":                ["road trip", "cross-country", "drives across", "journey across", "on the road"],
    "buddy_comedy":             ["buddy comedy", "unlikely duo", "mismatched pair", "two friends", "comedy duo"],
    "odd_couple":               ["odd couple", "mismatched", "unlikely pair", "opposites", "different worlds"],
    "enemies_to_lovers":        ["enemies to lovers", "starts as enemies", "hate to love", "rivals fall in love"],
    "friends_to_lovers":        ["friends to lovers", "childhood friends", "best friends fall in love", "longtime friends"],
    "love_at_first_sight":      ["love at first sight", "instantly falls for", "immediately attracted"],
    "unrequited_love":          ["unrequited love", "one-sided love", "loves someone who", "feelings not returned"],
    "long_distance_relationship":["long distance", "separated by distance", "far apart", "reunite"],
    "breakup_and_reconciliation":["breakup", "breaks up", "reconcile", "get back together", "second chance at love"],
    "marriage_struggles":       ["marriage", "divorce", "marital problems", "troubled marriage", "falling apart"],
    "parenting_challenges":     ["parenting", "parent", "father", "mother", "raise a child", "single parent"],
    "adoption":                 ["adoption", "adopted", "adoptive", "foster child", "takes in a child"],
    "lost_child":               ["lost child", "missing child", "kidnapped child", "find their child"],
    "reunion_after_years":      ["reunited after years", "haven't seen in years", "long-lost", "reconnects after"],
    "secret_child":             ["secret child", "hidden child", "didn't know he had", "didn't know she had"],
    "hidden_inheritance":       ["inheritance", "inherits", "estate", "will reading", "heir"],
    "small_town_secrets":       ["small town", "close-knit community", "dark secrets", "town's secret", "everybody knows"],
    "big_city_dreams":          ["big city", "moves to the city", "city dreams", "makes it in the city"],
    "culture_clash":            ["culture clash", "cultural differences", "different cultures", "foreign land", "outsider"],
    "immigration_story":        ["immigrant", "immigration", "new country", "leaves his country", "leaves her country"],
    "identity_and_belonging":   ["belonging", "where he belongs", "where she belongs", "identity", "place in the world"],
    "social_injustice":         ["social injustice", "inequality", "discrimination", "oppression", "systemic"],
    "racism":                   ["racism", "racist", "racial", "segregation", "prejudice", "bigotry"],
    "class_divide":             ["class divide", "class system", "upper class", "lower class", "social class", "wealth gap"],
    "gender_roles":             ["gender roles", "sexism", "gender equality", "feminism", "breaking barriers"],
    "lgbtq_identity":           ["LGBTQ", "gay", "lesbian", "transgender", "queer", "bisexual", "coming out"],
    "activism":                 ["activism", "activist", "protest", "fight for rights", "social change", "movement"],
    "environmental_crisis":     ["environmental", "climate change", "pollution", "ecological", "nature under threat"],
    "pandemic":                 ["pandemic", "virus", "outbreak", "epidemic", "plague", "contagion"],
    "medical_drama":            ["medical", "hospital", "doctor", "surgeon", "diagnosis", "treatment", "patient"],
    "terminal_illness":         ["terminal illness", "dying", "terminal", "cancer", "fatal diagnosis", "months to live"],
    "miracle_cure":             ["miracle cure", "miraculous recovery", "defies the odds", "beats the disease"],
    "addiction":                ["addiction", "addicted", "substance abuse", "drugs", "alcohol", "recovery"],
    "recovery_journey":         ["recovery", "sobriety", "rehabilitation", "gets clean", "overcomes addiction"],
    "mental_health":            ["mental health", "depression", "anxiety", "trauma", "therapy", "psychiatrist"],
    "obsession":                ["obsession", "obsessed", "fixated", "can't let go", "stalker"],
    "paranoia":                 ["paranoia", "paranoid", "can't trust anyone", "everyone is against", "losing his mind"],
    "isolation":                ["isolation", "isolated", "alone", "cut off", "solitary", "stranded"],
    "cabin_in_the_woods":       ["cabin in the woods", "remote cabin", "isolated cabin", "woods", "forest retreat"],
    "stranded":                 ["stranded", "marooned", "stuck", "no way out", "can't escape"],
    "survival_against_nature":  ["survival against nature", "nature", "wilderness survival", "elements", "harsh environment"],
    "survival_against_odds":    ["against all odds", "impossible situation", "survival", "fight to stay alive"],
    "lost_in_wilderness":       ["lost in the wilderness", "lost in the jungle", "lost in the forest", "stranded in nature"],
    "shipwreck":                ["shipwreck", "ship sinks", "stranded at sea", "castaway", "lost at sea"],
    "mythology_inspired":       ["mythology", "mythological", "myth", "legend", "gods", "ancient legend"],
    "gods_among_humans":        ["gods among humans", "deity", "divine being", "god walks", "immortal among mortals"],
    "magic_school":             ["magic school", "school of magic", "wizarding", "academy for", "trains young"],
    "forbidden_magic":          ["forbidden magic", "dark magic", "banned spell", "dangerous power", "outlawed"],
    "dark_fantasy":             ["dark fantasy", "grim", "dark world", "sinister", "bleak"],
    "epic_fantasy":             ["epic fantasy", "fantasy world", "magical realm", "fantastical", "mythical land"],
    "sword_and_sorcery":        ["sword and sorcery", "swords", "sorcerer", "wizard", "magic and combat"],
    "kingdom_politics":         ["kingdom", "king", "queen", "throne", "royal court", "monarchy"],
    "royal_intrigue":           ["royal intrigue", "court intrigue", "palace politics", "royal family", "noble"],
    "succession_battle":        ["succession", "heir", "throne", "who will rule", "fight for the crown"],
    "assassination_plot":       ["assassination", "assassin", "plot to kill", "murder plot", "hired killer"],
    "bodyguard_duty":           ["bodyguard", "protects", "keeps safe", "assigned to protect", "security detail"],
    "kidnapping":               ["kidnapping", "kidnapped", "abducted", "taken hostage", "missing person"],
    "rescue_mission":           ["rescue mission", "rescue operation", "save them", "goes to rescue", "must save"],
    "hostage_situation":        ["hostage", "taken hostage", "held captive", "demands ransom", "negotiator"],
    "chase":                    ["chase", "pursuit", "on the run", "hunted", "fleeing"],
    "cat_and_mouse_game":       ["cat and mouse", "hunter and hunted", "game of cat", "pursuer", "evading"],
    "race_against_time":        ["race against time", "running out of time", "before it's too late", "deadline"],
    "countdown_scenario":       ["countdown", "ticking clock", "limited time", "hours to stop"],
    "ticking_bomb":             ["bomb", "explosive", "detonation", "defuse", "ticking"],
    "secret_society":           ["secret society", "secret organization", "shadowy group", "brotherhood", "sisterhood"],
    "cult":                     ["cult", "cult leader", "brainwashed", "commune", "indoctrinated"],
    "ritual":                   ["ritual", "ceremony", "ancient rite", "sacrifice", "occult"],
    "ancient_evil":             ["ancient evil", "awakened evil", "dormant evil", "centuries old", "primordial"],
    "awakening_power":          ["awakening power", "discovers his power", "discovers her power", "unlocks abilities", "powers emerge"],
    "superhero_origin":         ["superhero origin", "gains powers", "becomes a hero", "origin story", "radioactive"],
    "superhero_team_up":        ["superhero team", "heroes unite", "team of heroes", "assemble", "combine forces"],
    "vigilante_justice":        ["vigilante", "takes the law", "own hands", "outside the law", "street justice"],
    "power_corruption":         ["power corrupts", "corrupted by power", "drunk on power", "absolute power"],
    "hidden_abilities":         ["hidden abilities", "secret powers", "latent abilities", "discovers he can", "discovers she can"],
    "clone_story":              ["clone", "cloning", "genetic duplicate", "copy of himself", "copy of herself"],
    "genetic_experiment":       ["genetic experiment", "genetically modified", "DNA", "gene", "experiment on humans"],
    "scientific_breakthrough":  ["scientific breakthrough", "discovery", "invention", "scientist", "experiment succeeds"],
    "ethical_science_dilemma":  ["ethical dilemma", "playing god", "science gone wrong", "unintended consequences"],
    "virtual_reality":          ["virtual reality", "VR", "simulated world", "digital world", "matrix"],
    "simulation_theory":        ["simulation", "simulated reality", "is it real", "nothing is real"],
    "game_world":               ["game world", "video game", "trapped in a game", "game come to life"],
    "reality_vs_illusion":      ["reality vs illusion", "what is real", "can't tell what's real", "illusion"],
    "memory_manipulation":      ["memory manipulation", "false memories", "memories erased", "implanted memories"],
    "surveillance_state":       ["surveillance", "big brother", "watched", "monitored", "tracked"],
    "hacker_story":             ["hacker", "hacking", "cyber", "breaks into", "digital infiltration"],
    "cybercrime":               ["cybercrime", "cybercriminal", "online fraud", "digital theft", "dark web"],
    "identity_theft":           ["identity theft", "steals his identity", "steals her identity", "impostor", "assumes identity"],
    "imposter":                 ["imposter", "impostor", "pretending to be", "not who they seem", "fake identity"],
    "hidden_agenda":            ["hidden agenda", "secret motive", "ulterior motive", "not what it seems"],
    "redemption_through_sacrifice": ["sacrifices himself", "sacrifices herself", "gives his life", "gives her life", "ultimate sacrifice"],
    "bittersweet_ending":       ["bittersweet", "not a happy ending", "mixed ending", "victory comes at a cost"],
    "twist_ending":             ["twist ending", "shocking ending", "twist", "unexpected ending", "didn't see coming"],
    "open_ending":              ["open ending", "ambiguous ending", "left to interpretation", "unresolved"],
    "full_circle_ending":       ["full circle", "comes full circle", "where it began", "back to the beginning"],
}


def get_connection():
    return psycopg2.connect(**DB_CONFIG)


def fetch_movies(conn, limit=None, imdb_id=None):
    cur = conn.cursor()
    if imdb_id:
        cur.execute(
            "SELECT imdb_id, title, plot_summary FROM movies WHERE imdb_id = %s AND plot_summary IS NOT NULL AND plot_summary != ''",
            (imdb_id,)
        )
    elif limit:
        cur.execute(
            "SELECT imdb_id, title, plot_summary FROM movies WHERE plot_summary IS NOT NULL AND plot_summary != '' LIMIT %s",
            (limit,)
        )
    else:
        cur.execute(
            "SELECT imdb_id, title, plot_summary FROM movies WHERE plot_summary IS NOT NULL AND plot_summary != ''"
        )
    return cur.fetchall()


def fetch_plot_tags(conn):
    cur = conn.cursor()
    cur.execute("SELECT plot_tag_id, tag_text_norm FROM plot_tags")
    return {row[1]: row[0] for row in cur.fetchall()}  # norm -> id


def match_tags(plot_summary: str) -> list[str]:
    """Return list of tag_text_norm values that match the plot summary."""
    text = plot_summary.lower()
    matched = []
    for tag_norm, keywords in TAG_KEYWORDS.items():
        for kw in keywords:
            if kw.lower() in text:
                matched.append(tag_norm)
                break  # only need one keyword to match per tag
    return matched


def insert_movie_plot_tags(conn, imdb_id: str, tag_ids: list[int], dry_run: bool):
    if not tag_ids:
        return 0
    cur = conn.cursor()
    inserted = 0
    for tag_id in tag_ids:
        if dry_run:
            inserted += 1
        else:
            cur.execute(
                """
                INSERT INTO movie_plot_tags (imdb_id, plot_tag_id, created_at, created_by_user_id, status)
                VALUES (%s, %s, NOW(), NULL, 'approved')
                ON CONFLICT (imdb_id, plot_tag_id) DO NOTHING
                """,
                (imdb_id, tag_id)
            )
            inserted += cur.rowcount
    if not dry_run:
        conn.commit()
    return inserted


def main():
    parser = argparse.ArgumentParser(description="Auto-tag movies using keyword matching.")
    group = parser.add_mutually_exclusive_group(required=True)
    group.add_argument("--limit", type=int, help="Tag a random batch of N movies")
    group.add_argument("--imdb", type=str, help="Tag a single movie by IMDB ID")
    group.add_argument("--all", action="store_true", help="Tag all movies")
    parser.add_argument("--dry-run", action="store_true", help="Show matches without writing to DB")
    args = parser.parse_args()

    conn = get_connection()
    tag_lookup = fetch_plot_tags(conn)  # norm -> id

    if args.imdb:
        movies = fetch_movies(conn, imdb_id=args.imdb)
    elif args.limit:
        movies = fetch_movies(conn, limit=args.limit)
    else:
        movies = fetch_movies(conn)

    print(f"\n{'DRY RUN - ' if args.dry_run else ''}Processing {len(movies)} movie(s)...\n")

    total_inserted = 0
    for imdb_id, title, plot_summary in movies:
        matched_norms = match_tags(plot_summary)
        tag_ids = [tag_lookup[n] for n in matched_norms if n in tag_lookup]

        print(f"  {title} ({imdb_id})")
        if matched_norms:
            for norm in matched_norms:
                print(f"    + {norm}")
        else:
            print(f"    (no tags matched)")

        count = insert_movie_plot_tags(conn, imdb_id, tag_ids, dry_run=args.dry_run)
        total_inserted += count

    print(f"\n{'Would insert' if args.dry_run else 'Inserted'} {total_inserted} tag(s) across {len(movies)} movie(s).")
    conn.close()


if __name__ == "__main__":
    main()