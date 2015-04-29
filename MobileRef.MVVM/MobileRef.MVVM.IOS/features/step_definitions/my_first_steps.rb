Given /^I am on the Welcome Screen$/ do
  element_exists("view")
  sleep(STEP_PAUSE)
end


Then /^I wait for  "([^\"]*)" $/ do |name|
  wait_for(WAIT_TIMEOUT) { element_exists( "button id:'#{name}'" ) }
end

Then /^I click" $/ do 
   macro 'I touch "btnSimpleMessage"'
end
