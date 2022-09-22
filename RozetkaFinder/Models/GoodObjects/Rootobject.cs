public class Rootobject
{
    public Data data { get; set; }
}

public class Data
{
    public string empty_response_type { get; set; }
    public string empty_response_message { get; set; }
    public Quantities quantities { get; set; }
    public Pagination pagination { get; set; }
    public Option[] options { get; set; }
    public object[] chosen { get; set; }
    public object[] related_options { get; set; }
    public Meta meta { get; set; }
    public Categories categories { get; set; }
    public Good[] goods { get; set; }
}

public class Quantities
{
    public int goods_quantity_max { get; set; }
    public int goods_quantity_found { get; set; }
    public int goods_quantity_total_found { get; set; }
}

public class Pagination
{
    public int page_size { get; set; }
    public int show_next { get; set; }
    public int shown_page { get; set; }
    public int total_pages { get; set; }
}

public class Meta
{
    public Result_Info result_info { get; set; }
    public string h1 { get; set; }
    public string title { get; set; }
    public string keywords { get; set; }
    public string description { get; set; }
    public object navigateTo { get; set; }
}

public class Result_Info
{
    public bool is_part { get; set; }
    public object wrong_phrase { get; set; }
    public object wrong_keyboard_text { get; set; }
}

public class Categories
{
    public object[] hidden_categories { get; set; }
    public List_Categories[] list_categories { get; set; }
    public Config config { get; set; }
    public object current_category { get; set; }
    public string option_title { get; set; }
    public string option_name { get; set; }
    public string special_combobox_view { get; set; }
    public int goods_quantity { get; set; }
}

public class Config
{
    public bool show_icon { get; set; }
    public bool show_hidden_categories { get; set; }
}

public class List_Categories
{
    public Child[] children { get; set; }
    public string icon { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public int count { get; set; }
}

public class Child
{
    public bool hide_cat { get; set; }
    public object[] children { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public int count { get; set; }
}

public class Option
{
    public bool hide_block { get; set; }
    public string option_id { get; set; }
    public string option_name { get; set; }
    public string option_title { get; set; }
    public string option_type { get; set; }
    public string special_combobox_view { get; set; }
    public string title_accusative { get; set; }
    public string title_genetive { get; set; }
    public string title_prepositional { get; set; }
    public string category_id { get; set; }
    public string comparable { get; set; }
    public string more_word { get; set; }
    public float order { get; set; }
    public bool is_checkbox { get; set; }
    public Option_Values[] option_values { get; set; }
    public Short_List[] short_list { get; set; }
    public int total_filtered { get; set; }
    public int total_found { get; set; }
    public int? count_rank_options { get; set; }
    public int? count_rank_active_options { get; set; }
    public string option_value_unit { get; set; }
    public bool isDecimal { get; set; }
    public Config1 config { get; set; }
    public Range_Values range_values { get; set; }
    public object chosen_values { get; set; }
}

public class Config1
{
    public int maxLength { get; set; }
    public string valuesPattern { get; set; }
    public Limit limit { get; set; }
    public string unit { get; set; }
}

public class Limit
{
    public int min { get; set; }
    public int max { get; set; }
}

public class Range_Values
{
    public int min { get; set; }
    public int max { get; set; }
}

public class Option_Values
{
    public object option_value_id { get; set; }
    public string option_value_name { get; set; }
    public string option_value_title { get; set; }
    public float order { get; set; }
    public object products_quantity { get; set; }
    public bool disabled { get; set; }
    public bool is_chosen { get; set; }
    public bool is_rank { get; set; }
    public string title_accusative { get; set; }
    public string title_genetive { get; set; }
    public string title_prepositional { get; set; }
    public bool is_checkbox { get; set; }
    public bool not_hidden { get; set; }
}

public class Short_List
{
    public object option_value_id { get; set; }
    public string option_value_name { get; set; }
    public string option_value_title { get; set; }
    public float order { get; set; }
    public object products_quantity { get; set; }
    public bool disabled { get; set; }
    public bool is_chosen { get; set; }
    public bool is_rank { get; set; }
    public string title_accusative { get; set; }
    public string title_genetive { get; set; }
    public string title_prepositional { get; set; }
    public bool is_checkbox { get; set; }
    public bool not_hidden { get; set; }
}

public class Good
{
    public int id { get; set; }
    public string title { get; set; }
    public string href { get; set; }
    public string docket { get; set; }
    public bool is_docket { get; set; }
    public Description_Fields description_fields { get; set; }
    public int category_id { get; set; }
    public int comments_amount { get; set; }
    public float comments_mark { get; set; }
    public int merchant_id { get; set; }
    public int old_price { get; set; }
    public int pl_bonus_charge_pcs { get; set; }
    public int pl_use_instant_bonus { get; set; }
    public int price { get; set; }
    public string price_pcs { get; set; }
    public string sell_status { get; set; }
    public string status { get; set; }
    public int seller_id { get; set; }
    public string state { get; set; }
    public string image_main { get; set; }
    public Images images { get; set; }
    public int parent_category_id { get; set; }
    public string brand { get; set; }
    public int brand_id { get; set; }
    public int producer_id { get; set; }
    public int discount { get; set; }
    public string stars { get; set; }
    public Groups groups { get; set; }
    public bool is_groups_title { get; set; }
    public Config2 config { get; set; }
}

public class Description_Fields
{
    public _20860 _20860 { get; set; }
}

public class _20860
{
    public int opt_id { get; set; }
    public string o_title { get; set; }
    public string go_value { get; set; }
}

public class Images
{
    public string main { get; set; }
    public string hover { get; set; }
    public string preview { get; set; }
}

public class Groups
{
    public object block { get; set; }
    public Color[] color { get; set; }
    public Prices prices { get; set; }
}

public class Prices
{
    public int count { get; set; }
    public string href { get; set; }
    public int min { get; set; }
    public int max { get; set; }
}

public class Color
{
    public Color1 color { get; set; }
    public bool active_option { get; set; }
    public int id { get; set; }
    public string href { get; set; }
    public int order { get; set; }
    public string sell_status { get; set; }
    public int seller_id { get; set; }
    public string value { get; set; }
}

public class Color1
{
    public int id { get; set; }
    public string hash { get; set; }
    public string url { get; set; }
}

public class Config2
{
    public bool bonus { get; set; }
    public bool brand { get; set; }
    public bool buy_button { get; set; }
    public bool compare_button { get; set; }
    public bool description { get; set; }
    public bool gift { get; set; }
    public bool image { get; set; }
    public bool old_price { get; set; }
    public bool pictograms { get; set; }
    public bool price { get; set; }
    public bool promo_price { get; set; }
    public bool rating { get; set; }
    public bool status { get; set; }
    public bool tags { get; set; }
    public bool title { get; set; }
    public bool variables { get; set; }
    public bool wishlist_button { get; set; }
    public bool promo_code { get; set; }
    public bool hide_rating_review { get; set; }
    public bool show_supermarket_price { get; set; }
}
